use axum::{Json, extract::Query, http::StatusCode};
use chrono::NaiveDate;
use log::{info, warn};
use serde::{Deserialize, Serialize};

use crate::models::{Equipment, TimeSpan};


pub async fn get_availability(req: Query<AvailabilityRequest>) -> (StatusCode, Json<AvailabilityResponse>) {
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

#[derive(Serialize, Deserialize, Debug)]
pub struct AvailabilityRequest {
    guid: String,
    date: NaiveDate,
}


#[derive(Serialize)]
pub enum AvailabilityResponse {
    Available,
    NotAvailable,
    EquipmentNotFound,
}
