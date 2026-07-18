
use axum::Json;
use serde::Deserialize;

use crate::{
    AppError,
    repository::{Equipment, Repository},
};

pub async fn post_equipment(Json(payload): Json<EquipmentData>) -> Result<(), AppError> {
    let mut repository =
        Repository::establish_connection().map_err(|_e| AppError::DatabaseConnectionError)?;

    repository
        .create_equipment(payload)
        .map_err(|_e| AppError::FailedToReadEquipment)?;
    Ok(())
}

pub async fn get_equipment() -> Result<Json<Vec<Equipment>>, AppError> {

    let mut repository =
        Repository::establish_connection().map_err(|_e| AppError::DatabaseConnectionError)?;

    let equipment = repository
        .read_equipment()
        .map_err(|_e| AppError::FailedToReadEquipment)?;

    Ok(Json(equipment))
}

#[derive(Deserialize, Debug)]
pub struct EquipmentData {
    pub name: String,
    pub description: Option<String>,
}
