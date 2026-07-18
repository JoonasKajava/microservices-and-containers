// @generated automatically by Diesel CLI.

diesel::table! {
    equipment (id) {
        id -> Uuid,
        #[max_length = 255]
        name -> Varchar,
        #[max_length = 1024]
        description -> Nullable<Varchar>,
    }
}
