mod availability;
mod equipment;
mod models;
mod repository;
mod schema;

use std::env;

use axum::{
    Router, http::StatusCode, response::{IntoResponse, Response}, routing::{get, post}
};
use log::{error, info};
use tokio::net::TcpListener;

#[tokio::main]
async fn main() {
    pretty_env_logger::formatted_timed_builder()
        .filter_level(log::LevelFilter::Info)
        .parse_default_env()
        .init();

    let bind_addr = env::var("INVENTORY_BIND_ADDR").unwrap_or("0.0.0.0:3000".to_string());

    info!("Binding to {}", bind_addr);

    // TODO: Improve error handling
    let app = Router::new()
        .route("/api/v1/availability", get(availability::get_availability))
        .route("/api/v1/equipment", get(equipment::get_equipment))
        .route("/api/v1/equipment", post(equipment::post_equipment));

    match TcpListener::bind(&bind_addr).await {
        Ok(listener) => match axum::serve(listener, app).await {
            Ok(()) => info!("Shutdown"),
            Err(e) => error!("Failed to serve: {}", e),
        },
        Err(e) => error!("Failed to bind {} with error: {}", bind_addr, e),
    }
}

#[derive(Debug)]
enum AppError {
    FailedToReadEquipment,
    FailedToCreateEquipment,
    DatabaseConnectionError
}

impl IntoResponse for AppError {
    fn into_response(self) -> Response {
        (
            StatusCode::INTERNAL_SERVER_ERROR,
            format!("Something went wrong: {:?}", self),
        )
            .into_response()
    }
}
