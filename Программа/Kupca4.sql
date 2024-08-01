create database [is1-24-hay_k]
use [is1-24-hay_k]
create table Types_Users(
id_type int primary key not null,
name_type nvarchar(40));

create table Users(
id_User int primary key identity,
Login nvarchar(18),
password nvarchar(20),
type_user int references Types_Users(id_type));

create table Clinets(
id_client int primary key,
id_user int references Users(id_User),
path_to_the_file nvarchar(200),
name nvarchar(50),
Surname nvarchar(70));

create table Chat(
id_chat int primary key identity,
id_user_one int references Users(id_User),
id_user_two int references Users(id_User));

create table Project(
id_project int primary key identity,
path_to_cat nvarchar(200));

create table U_P(
users int references Users(id_user),
boss int references Users(id_user),
project int references Project(id_project));

create table Message(
id_message int primary key identity,
id_chat int references Chat(id_chat),
files nvarchar(200),
sendler int references Users(id_user),
content nvarchar(max),
date_send datetime);

insert into Types_Users
values ('0','Администратор'),
		('1','Модератор'),
		('2','Пользователь');

insert into Users
values ('Admin','admin_top','0');

insert into Users
values ('user_1','123456',2),
('user_2','123456',2);

insert into Chat
values (1,2),(2,3),(1,3)


insert into [Message]
values (1,'',1,'Привет','2021-12-12'),
(2,'',2,'Привет','2021-12-12'),
(3,'',1,'Привет','2021-12-12')