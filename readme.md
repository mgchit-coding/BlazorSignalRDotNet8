```
create table Tbl_User
(
    UserId          int identity
        constraint Tbl_User_pk
            primary key,
    GeneratedUserId nvarchar(50) not null,
    UserName        nvarchar(50) not null,
    Password        nvarchar(50) not null,
    UserType        nvarchar(50) not null
)
go
```

```
create table Tbl_Login
(
    LoginId      int identity
        constraint Tbl_Login_pk
            primary key,
    UserId       nvarchar(50) not null,
    SessionId    nvarchar(50) not null,
    ConnectionId nvarchar(50)
)
go


```


