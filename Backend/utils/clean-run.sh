# Флаг -v удаляет анонимные и именованные тома, привязанные к контейнерам
docker-compose down -v

# Удаляет все неиспользуемые контейнеры, сети, образы (включая "висячие")
docker system prune -a -f

docker-compose build --no-cache

docker-compose up -d

docker-compose ps

docker-compose logs -f server