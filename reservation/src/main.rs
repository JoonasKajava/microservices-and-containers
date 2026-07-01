use std::thread;

use zmq::{Context, Message, SocketType::REQ};

fn main() {
    let context = Context::new();

    let requester = context.socket(REQ).unwrap();

    requester.connect("tcp://localhost:5555").unwrap();

    let mut msg = Message::new();

    loop {
        println!("Sending request...");

        requester.send("Hello", 0).unwrap();

        requester.recv(&mut msg, 0).unwrap();

        println!("Received reply: {}", msg.as_str().unwrap());

        thread::sleep(std::time::Duration::from_secs(1));
    }
}
