
use std::{thread::Thread, time::Duration};

use axum::{Json, http::StatusCode};
use serde::Deserialize;
use tokio::time::sleep;

pub async fn post_equipment(
    Json(payload): Json<EquipmentData>,
)  -> (StatusCode, String) {
    sleep(Duration::from_secs(3)).await;
    (StatusCode::NOT_IMPLEMENTED, "Adding Equipment is not implemented yet".to_string())
    // Ok((StatusCode::OK, "".to_string()))
}

#[derive(Deserialize, Debug)]
pub struct EquipmentData {}
