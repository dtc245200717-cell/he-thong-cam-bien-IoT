# Đề tài 06 - Hệ thống giám sát cảm biến IoT

## 1. Mô tả đề tài

Xây dựng hệ thống tiếp nhận dữ liệu từ nhiều cảm biến giả lập, lưu trữ dữ liệu và hiển thị trạng thái gần thời gian thực. Khi giá trị vượt ngưỡng cấu hình, hệ thống phát cảnh báo đến client.

Kiến trúc tối thiểu của hệ thống:

```text
Sensor Clients -> Sensor API -> RabbitMQ -> Monitor Worker -> Database
                                      |
                                      +-> Notification Service -> Dashboard Client
```

## 2. Chức năng chính

1. Đăng ký thông tin cảm biến.
2. Tiếp nhận dữ liệu đo từ nhiều cảm biến giả lập.
3. Đưa dữ liệu đo vào hàng đợi để xử lý.
4. Worker lưu và kiểm tra dữ liệu theo ngưỡng cảnh báo.
5. Phát sự kiện khi có giá trị vượt ngưỡng.
6. Gửi cảnh báo realtime đến client giám sát.
7. Tra cứu dữ liệu gần nhất của từng cảm biến.
8. Tra cứu lịch sử dữ liệu theo khoảng thời gian.

## 3. API tối thiểu

```text
POST /sensors
POST /sensors/{id}/readings
GET  /sensors
GET  /sensors/{id}/readings/latest
GET  /sensors/{id}/readings
```

Dữ liệu tối thiểu gồm cảm biến, thời điểm đo, giá trị đo, ngưỡng cảnh báo và trạng thái cảnh báo.
