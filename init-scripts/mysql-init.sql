-- Script para MySQL
-- UUIDs são armazenados como BINARY(16) para melhor performance

CREATE TABLE appointments (
  id BINARY(16) PRIMARY KEY DEFAULT (UUID_TO_BIN(UUID())),
  client_id BINARY(16) NOT NULL,
  professional_id BINARY(16) NOT NULL,
  service_id BINARY(16) NOT NULL,
  start_at DATETIME NOT NULL,
  duration_minutes INT NOT NULL,
  status VARCHAR(50) NOT NULL,
  created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);