-- Script para PostgreSQL
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE appointments (
  id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  client_id UUID NOT NULL,
  professional_id UUID NOT NULL,
  service_id UUID NOT NULL,
  start_at TIMESTAMP WITH TIME ZONE NOT NULL,
  duration_minutes INT NOT NULL,
  status VARCHAR(50) NOT NULL,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT now()
);