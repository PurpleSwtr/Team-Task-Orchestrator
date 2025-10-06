GREEN='\033[0;32m'
NC='\033[0m' 

echo -e "${GREEN}Шаг 1: Останавливаем и удаляем все контейнеры, сети и тома (volumes)...${NC}"
# Флаг -v удаляет анонимные и именованные тома, привязанные к контейнерам
docker-compose down -v

echo -e "${GREEN}Шаг 2: Принудительная очистка системы Docker от старых образов и кэша сборки...${NC}"
# Удаляет все неиспользуемые контейнеры, сети, образы (включая "висячие")
docker system prune -a -f

echo -e "${GREEN}Шаг 3: Пересобираем образы с нуля, без использования кэша...${NC}"
docker-compose build --no-cache

echo -e "${GREEN}Шаг 4: Запускаем все сервисы в фоновом режиме...${NC}"
docker-compose up -d

echo -e "${GREEN}Готово! Проверяем статус контейнеров:${NC}"
docker-compose ps

echo -e "${GREEN}Смотрим логи API-сервера (нажми Ctrl+C для выхода):${NC}"
docker-compose logs -f server