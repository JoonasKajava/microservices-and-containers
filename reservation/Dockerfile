FROM rust:1.95-alpine AS builder

COPY ./src ./src
COPY ./Cargo.lock .
COPY ./Cargo.toml .

RUN apk add --no-cache g++ lld musl-dev git

RUN rustup target add x86_64-unknown-linux-musl

RUN cargo build --target x86_64-unknown-linux-musl --release

FROM debian:bullseye-slim
COPY --from=builder ./target/x86_64-unknown-linux-musl/release/reservation /app/reservation
ENTRYPOINT ["./app/reservation"]
