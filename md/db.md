[INITIALIZE_NETWORK]
docker network create ocr_network

[DEV_DB]
docker-compose --env-file .env.development -f docker-compose-db.dev.yml up --build -d

[TEST_DB_DOCKER]
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "DuyNguyen@123"
