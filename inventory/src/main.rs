use std::env;

use axum::{Json, Router, extract::Query, http::StatusCode, routing::get};
use chrono::NaiveDate;
use log::{error, info, warn};
use serde::{Deserialize, Serialize};
use tokio::net::TcpListener;

#[tokio::main]
async fn main() {
    pretty_env_logger::formatted_timed_builder()
        .filter_level(log::LevelFilter::Info)
        .parse_default_env()
        .init();

    let bind_addr = env::var("INVENTORY_BIND_ADDR").unwrap_or("0.0.0.0:3000".to_string());

    info!("Binding to {}", bind_addr);

    let app = Router::new().route("/availability", get(availability));

    match TcpListener::bind(&bind_addr).await {
        Ok(listener) => match axum::serve(listener, app).await {
            Ok(()) => todo!(),
            Err(e) => error!("Failed to serve: {}", e),
        },
        Err(e) => error!("Failed to bind {} with error: {}", bind_addr, e),
    }
}

async fn availability(req: Query<AvailabilityRequest>) -> (StatusCode, Json<AvailabilityResponse>) {
    info!("Received request: {:?}", req);

    let equipment_db: Vec<Equipment> = vec![Equipment {
        id: "24549cf0-28b7-4553-8e38-9395a17cdd9e",
        name: "Laptop",
        reservations: vec![TimeSpan {
            start: NaiveDate::from_ymd_opt(2026, 7, 1).unwrap(),
            end: NaiveDate::from_ymd_opt(2026, 7, 3).unwrap(),
        }],
    }];

    match equipment_db.iter().find(|item| item.id == req.guid) {
        Some(equipment) if equipment.is_available(req.date) => {
            info!("Equipment {} is available", equipment.name);
            (StatusCode::OK, Json(AvailabilityResponse::Available))
        }
        Some(equipment) => {
            info!("Equipment {:?} is not available", equipment);
            (StatusCode::OK, Json(AvailabilityResponse::NotAvailable))
        },
        None => {
            warn!("Equipment with guid {} not found", req.guid);
            (
                StatusCode::NOT_FOUND,
                Json(AvailabilityResponse::EquipmentNotFound),
            )
        },
    }
}

#[derive(Serialize, Debug)]
struct Equipment<'a> {
    id: &'a str,
    name: &'a str,
    reservations: Vec<TimeSpan>,
}

impl<'a> Equipment<'a> {
    fn is_available(&self, date: NaiveDate) -> bool {
        self.reservations
            .iter()
            .all(|reservation| !reservation.contains(date))
    }
}



#[derive(Serialize, Debug)]
struct TimeSpan {
    pub start: NaiveDate,
    pub end: NaiveDate,
}

impl TimeSpan {
    pub fn contains(&self, time: NaiveDate) -> bool {
        self.start <= time && time <= self.end
    }
}


#[derive(Serialize, Deserialize, Debug)]
struct AvailabilityRequest {
    guid: String,
    date: NaiveDate,
}


#[derive(Serialize)]
enum AvailabilityResponse {
    Available,
    NotAvailable,
    EquipmentNotFound,
}
