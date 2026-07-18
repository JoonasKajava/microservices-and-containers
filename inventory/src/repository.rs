use std::env;

use anyhow::Result;
use serde::Serialize;
use uuid::Uuid;

use diesel::{prelude::*};

use crate::{equipment::EquipmentData};

pub struct Repository {
    connection: PgConnection,
}

impl Repository {
    pub fn establish_connection() -> Result<Repository> {
        let connection_string = env::var("DATABASE_URL")?;
        let connection = PgConnection::establish(&connection_string)?;
        Ok(Repository { connection })
    }

    pub fn create_equipment(&mut self, equipment_data: EquipmentData) -> Result<usize> {
        use crate::schema::equipment;
        
        let new_equipment = NewEquipment {
            name: equipment_data.name,
            description: equipment_data.description
        };

        let result = diesel::insert_into(equipment::table).values(new_equipment).execute(&mut self.connection)?;
        Ok(result)
    }

    pub fn read_equipment(&mut self) -> Result<Vec<Equipment>> {
        use crate::schema::equipment::dsl::*;

        let results = equipment
            .select(Equipment::as_select())
            .load(&mut self.connection)?;

        Ok(results)
    }
}

#[derive(Queryable, Selectable, Serialize)]
#[diesel(table_name = crate::schema::equipment)]
#[diesel(check_for_backend(diesel::pg::Pg))]
pub struct Equipment {
    pub id: Uuid,
    pub name: String,
    pub description: Option<String>,
}

#[derive(Insertable)]
#[diesel(table_name = crate::schema::equipment)]
pub struct NewEquipment {
    pub name: String,
    pub description: Option<String>,
}
