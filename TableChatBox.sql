create table Categories
(
	Id varchar(36) not null,
	NameCategory nvarchar(100) not null,
	IntentCodeEN nvarchar(255) not null unique,
	IntentCodeVN nvarchar(255) not null unique,
	Description nvarchar(1000),
	ParentID nvarchar(36),
	Inactive bit not null,
	OnDelete bit not null

	constraint pk_id primary key (Id)
)

create table Intents
(
	Id varchar(36) not null,
	CategoryID nvarchar(36),
	IntentEN nvarchar(255),
	IntentVN nvarchar(255),
	Inactive bit not null,
	OnDelete bit not null
	constraint pk_id_i primary key(Id),
	CONSTRAINT fk_cate
	FOREIGN KEY (CategoryID) REFERENCES Categories(Id)
)

create table Answers
(
	Id varchar(36) not null,
	CategoryID nvarchar(36),
	AnswerVN nvarchar(1000),
	Inactive bit not null,
	OnDelete bit not null
	constraint pk_id_a primary key(Id),
	constraint fk_c foreign key (CategoryID) references Categories(Id)
)
