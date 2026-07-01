use serde::Serialize;
use zmq::{Context, Message, SocketType::REP};

fn main() {
    let context = Context::new();
    let responder = context.socket(REP).unwrap();

    responder.bind("tcp://*:5555").unwrap();

    let mut msg = Message::new();
    loop {
        responder.recv(&mut msg, 0).unwrap();

        println!("Received request: {}", msg.as_str().unwrap());

        let equipment = Equipment {
            id: 1,
            name: "Sword".to_string(),
        };

        let response = serde_json::to_string(&equipment).unwrap();

        responder.send(&response, 0).unwrap();
    }
}

#[derive(Serialize)]
struct Equipment {
    id: usize,
    name: String,
}
