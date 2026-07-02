FROM rust:1.95-alpine AS builder

COPY ./src ./src
COPY ./Cargo.lock .
COPY ./Cargo.toml .

RUN apk add --no-cache g++ lld musl-dev git openssl-dev

RUN rustup target add x86_64-unknown-linux-musl

RUN cargo build --target x86_64-unknown-linux-musl --release

FROM debian:bullseye-slim
COPY --from=builder ./target/x86_64-unknown-linux-musl/release/inventory /app/inventory
ENTRYPOINT ["./app/inventory"]
