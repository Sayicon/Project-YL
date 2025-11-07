#!/bin/bash

echo "Building Docker image..."
docker build -t minishell .

echo "Starting minishell in Docker..."
docker run -it --rm minishell
