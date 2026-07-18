use chrono::NaiveDate;
use serde::Serialize;

#[derive(Serialize, Debug)]
pub struct Equipment<'a> {
    pub id: &'a str,
    pub name: &'a str,
    pub reservations: Vec<TimeSpan>,
}

impl<'a> Equipment<'a> {
    pub fn is_available(&self, date: NaiveDate) -> bool {
        self.reservations
            .iter()
            .all(|reservation| !reservation.contains(date))
    }
}


#[derive(Serialize, Debug)]
pub struct TimeSpan {
    pub start: NaiveDate,
    pub end: NaiveDate,
}

impl TimeSpan {
    pub fn contains(&self, time: NaiveDate) -> bool {
        self.start <= time && time <= self.end
    }
}
