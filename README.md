# ToDoApp

Một ứng dụng danh sách công việc hiện đại, giúp quản lý công việc hiệu quả. Backend được xây dựng bằng **ASP.NET Core** với **Kiến trúc Layer** và **Mẫu Mediator** để đảm bảo mã nguồn sạch và dễ mở rộng. Frontend được phát triển bằng **ReactJS**, cung cấp giao diện người dùng responsive và trực quan. Dữ liệu công việc được lưu trữ trong **Oracle Database**.

## Tính năng

- **Thêm công việc**: Tạo công việc mới với tiêu đề và mô tả tùy chọn.
- **Chỉnh sửa công việc**: Cập nhật chi tiết công việc để giữ danh sách gọn gàng.
- **Xóa công việc**: Loại bỏ công việc không còn cần thiết.
- **Đánh dấu hoàn thành**: Theo dõi tiến độ công việc bằng cách đánh dấu hoàn thành.
- **Giao diện responsive**: Hoạt động mượt mà trên di động và máy tính.
- **Lưu trữ bền vững**: Công việc được lưu trong Oracle Database qua backend.
- **Kiến trúc sạch**: Backend sử dụng kiến trúc phân tầng và Mediator để tách biệt logic.

## Công nghệ sử dụng

### Backend

- **Framework**: ASP.NET Core
- **Kiến trúc**: Layered Architecture (Presentation, Application, Domain, Infrastructure)
- **Design Patten**: Mediator Pattern (sử dụng MediatR)
- **Cơ sở dữ liệu**: Oracle
- **Thư viện khác**: Dapper

### Frontend

- **Framework**: ReactJS
- **Quản lý trạng thái**: React Context
- **HTTP Client**: FetchJS để gọi API
- **Giao diện**: Tailwind CSS
- **Node.js**: Phiên bản 16 trở lên
- **Package manager**: npm

## Lý do chọn Oracle Database

- Phù hợp với hệ thống microservice.
- Hỗ trợ truy vấn nhanh và hiệu quả.
- Có khả năng quản lý dữ liệu lớn.
- Có khả năng mở rộng nếu như dữ liệu dần nhiều hơn.
- Cung cấp mã hóa dữ liệu và kiểm soát truy cập để bảo vệ thông tin công việc.
- Do em đang tìm hiểu về Oracle Database nên em chọn để sử dụng.

## Bắt đầu

### Yêu cầu

- **Backend**:
  - [.NET SDK](https://dotnet.microsoft.com/download) (phiên bản 8.0)
  - [Oracle Database](https://www.oracle.com/database/) (đã cài đặt và chạy)
  - Thư viện: [Oracle.ManagedDataAccess.Core](https://www.nuget.org/packages/Oracle.ManagedDataAccess.Core)
  - IDE: [Visual Studio](https://visualstudio.microsoft.com/)
- **Frontend**:
  - [Node.js](https://nodejs.org) (phiên bản 16 trở lên)
  - Trình quản lý gói: npm
- Git đã được cài đặt.

### Cài đặt

1. **Clone source**:
   ```bash
   git clone https://github.com/quyok808/ToDoApp.git
   cd ToDoApp
   ```
2. **Database setup**

- Tạo schema cho ToDoApp:

  ```sql
  -- USER SQL
  CREATE USER "C##TSK" IDENTIFIED BY "123"
  DEFAULT TABLESPACE "USERS"
  TEMPORARY TABLESPACE "TEMP";

  -- QUOTAS
  ALTER USER "C##TSK" QUOTA UNLIMITED ON "SYSTEM";
  ALTER USER "C##TSK" QUOTA UNLIMITED ON "USERS";

  -- SYSTEM PRIVILEGES
  GRANT ALTER ANY INDEX TO "C##TSK" ;
  GRANT DROP ANY SEQUENCE TO "C##TSK" ;
  GRANT ALTER SESSION TO "C##TSK" ;
  GRANT CREATE ANY SEQUENCE TO "C##TSK" ;
  GRANT ALTER ANY TABLE TO "C##TSK" ;
  GRANT CREATE SESSION TO "C##TSK" ;
  GRANT SELECT ANY TABLE TO "C##TSK" ;
  GRANT DELETE ANY TABLE TO "C##TSK" ;
  GRANT ALTER ANY SEQUENCE TO "C##TSK" ;
  GRANT CREATE TABLE TO "C##TSK" ;
  GRANT DROP ANY TABLE TO "C##TSK" ;
  GRANT SELECT ANY SEQUENCE TO "C##TSK" ;
  GRANT CREATE SEQUENCE TO "C##TSK" ;
  GRANT UPDATE ANY TABLE TO "C##TSK" ;
  GRANT CREATE ANY TABLE TO "C##TSK" ;
  GRANT INSERT ANY TABLE TO "C##TSK";

  -- CREATE SEQUENCE
  CREATE SEQUENCE  "C##TSK"."SEQ_TASKS"  MINVALUE 1 MAXVALUE 99999999 INCREMENT BY 1 START WITH 21 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;

  -- CREATE TABLE
  CREATE TABLE "C##TSK"."TASKS"
   (	"ID" NUMBER DEFAULT "C##TSK"."SEQ_TASKS"."NEXTVAL",
  "NAME" VARCHAR2(150 BYTE),
  "DESCRIPTION" VARCHAR2(250 BYTE),
  "HAVEDONE" NUMBER DEFAULT 0,
  "CREATEDAT" TIMESTAMP (6),
  "UPDATEDAT" TIMESTAMP (6),
  "ENABLE" NUMBER DEFAULT 1
   ) SEGMENT CREATION IMMEDIATE
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255
  NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS" ;

  COMMENT ON COLUMN "C##TSK"."TASKS"."NAME" IS 'Tiêu đề task';
  REM INSERTING into C##TSK.TASKS
  SET DEFINE OFF;
  Insert into C##TSK.TASKS (ID,NAME,DESCRIPTION,HAVEDONE,CREATEDAT,UPDATEDAT,ENABLE) values (16,'abc','123',0,to_timestamp('08-JUL-25 04.26.20.925000000 PM','DD-MON-RR HH.MI.SSXFF AM'),to_timestamp('08-JUL-25 05.02.40.907000000 PM','DD-MON-RR HH.MI.SSXFF AM'),1);
  Insert into C##TSK.TASKS (ID,NAME,DESCRIPTION,HAVEDONE,CREATEDAT,UPDATEDAT,ENABLE) values (19,'asdasd','asdasd',0,to_timestamp('08-JUL-25 04.52.44.559000000 PM','DD-MON-RR HH.MI.SSXFF AM'),to_timestamp('08-JUL-25 04.53.36.876000000 PM','DD-MON-RR HH.MI.SSXFF AM'),0);
  Insert into C##TSK.TASKS (ID,NAME,DESCRIPTION,HAVEDONE,CREATEDAT,UPDATEDAT,ENABLE) values (17,'asdasdasd','asdasdasd',1,to_timestamp('08-JUL-25 04.27.31.278000000 PM','DD-MON-RR HH.MI.SSXFF AM'),to_timestamp('08-JUL-25 05.03.40.443000000 PM','DD-MON-RR HH.MI.SSXFF AM'),1);
  Insert into C##TSK.TASKS (ID,NAME,DESCRIPTION,HAVEDONE,CREATEDAT,UPDATEDAT,ENABLE) values (18,'asdasdasdasdasd','asdadasdadsadsadsasdasd',0,to_timestamp('08-JUL-25 04.27.36.138000000 PM','DD-MON-RR HH.MI.SSXFF AM'),to_timestamp('08-JUL-25 04.52.37.956000000 PM','DD-MON-RR HH.MI.SSXFF AM'),1);

  ALTER TABLE "C##TSK"."TASKS" MODIFY ("ID" NOT NULL ENABLE);
  ```

- Cấu hình chuỗi kết nối trong `ToDoApp/appsettings.json`:

  ```json
  {
    "PLSQLServerEquipmentServiceConnection": "User Id=C##TSK;Password=123;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));Validate Connection=true;Pooling=true;Incr Pool Size=30;Min Pool Size=10;Max Pool Size=250;Connection Timeout=60"
  }
  ```

3. **Backend**:

- Chuyển đến thư mục backend:
  ```bash
  cd ToDoApp
  ```
- Cài đặt các gói phụ thuộc:
  ```bash
  dotnet restore
  ```
- Chạy backend:
  ```bash
  dotnet run
  ```

4. **Frontend**:

- Chuyển đến thư mục frontend:
  ```bash
  cd todoapp-client
  ```
- Cài đặt các gói phụ thuộc:
  ```bash
  npm install
  ```
- Chạy frontend:
  ```bash
  npm run dev
  ```

## Truy cập ứng dụng:

- **Backend API**: http://localhost:5001
- **Frontend**: http://localhost:5173

## Sử dụng

- Khởi động backend API và frontend server.
- Mở trình duyệt tại http://localhost:5173.
- Tạo, chỉnh sửa, xóa hoặc đánh dấu công việc đã hoàn thành.

## Cấu trúc dự án

### Backend

- **Presentation**: Chứa API controllers và endpoints.
- **Application**: Chứa các handler MediatR, commands, và queries.
- **Domain**: Chứa middlewares.
- **Shared**: Thư viện dùng chung.
- **Infrastructure**: Chứa repositories, interfaces, và các mô hình chuyển đổi dữ liệu (DTOs).

### Frontend

- **src/components**: Các thành phần trong trang.
- **src/pages**: Chứa các trang.
- **src/services**: Logic gọi API.
- **src/stores**: Quản lý trạng thái bằng React Context.
- **src/hook**: Quản lý logic trong function component.
