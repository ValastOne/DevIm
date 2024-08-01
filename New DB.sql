create database [is1-24-hay-devim]
go
use [is1-24-hay-devim]
go
create table Roles
(
	id int primary key identity,
	title nvarchar(100) 
);

create table Users
(
	id int primary key identity,
	surname nvarchar(100) ,
	name nvarchar(100),
	login nvarchar(100),
	password nvarchar(max),
	role int references Roles(id)
);

create table Chats
(
	id int primary key identity,
	title nvarchar(100),
	path_to_dir nvarchar(max)
);

create table [Messages]
(
	id int primary key identity,
	[content] nvarchar(max),
	date datetime,
	[file] nvarchar(100),
	sendler int references Users(id),
	chat int references Chats(id)
);

create table Project
(
	id int primary key identity,
	title nvarchar(100),
	path_to_dir nvarchar(100),
	boss int references Users(id)
);

create table Users_In_Proj
(
	id int primary key identity,
	id_user int references Users(id),
	id_proj int references Project(id)
	
);

create table Users_In_Chats
(
	id int primary key identity,
	id_user int references Users(id),
	id_chat int references Chats(id)	
);