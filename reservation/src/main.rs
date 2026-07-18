use std::{env, thread, time::Duration};

use anyhow::Result;
use chrono::NaiveDate;
use log::{error, info, warn};
use rand::seq::IndexedRandom;
use reqwest::{Client, StatusCode};
use serde::{Deserialize, Serialize};

#[tokio::main]
async fn main() -> Result<()> {
    pretty_env_logger::formatted_timed_builder()
        .filter_level(log::LevelFilter::Info)
        .parse_default_env()
        .init();

    let inventory_addr =
        env::var("INVENTORY_ADDR").unwrap_or("http://localhost:3000".to_string());

    let client = Client::new();
    loop {

        let random_day = rand::random_range(1..7);
        let equipment_guids = ["24549cf0-28b7-4553-8e38-9395a17cdd9e", "asdf"];
        let random_equipment = equipment_guids.choose(&mut rand::rng()).unwrap();

        let test = AvailabilityRequest {
            guid: random_equipment.to_string(),
            date: NaiveDate::from_ymd_opt(2026, 7, random_day).unwrap(),
        };

        info!("Requesting availability {:?}", test);

        let res = match client
            .get(format!("{}/api/availability", inventory_addr))
            .query(&test)
            .send()
            .await
        {
            Ok(res) => res,
            Err(e) => {
                error!("Request failed: {} ", e);
                thread::sleep(Duration::from_secs(1));
                continue;
            }
        };

        let status = res.status();

        let body = res.text().await?;

        match status {
            StatusCode::OK => info!("Response: {}", body),
            _ => warn!("Status code: {}, body: {}", status, body),
        }

        thread::sleep(Duration::from_secs(3));
    }
}


#[derive(Serialize, Deserialize, Debug)]
struct AvailabilityRequest {
    guid: String,
    date: NaiveDate,
}
