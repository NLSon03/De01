USE [master]
GO

CREATE DATABASE [QuanlySV]
GO

USE [QuanlySV]
GO

CREATE TABLE [dbo].[Lop] (
    MaLop CHAR(3) PRIMARY KEY,
    TenLop NVARCHAR(30) NOT NULL
);

CREATE TABLE [dbo].[Sinhvien](
	MaSV CHAR(6) PRIMARY KEY,
	HotenSV NVARCHAR(40) NOT NULL,
	NgaySinh DATETIME NOT NULL,
	MaLop CHAR(3) FOREIGN KEY REFERENCES [dbo].[Lop](MaLop)
)

INSERT INTO Lop(MaLop,TenLop) VALUES('001',N'Công nghệ thông tin')
INSERT INTO Lop(MaLop,TenLop) VALUES('002',N'Ngôn Ngữ Anh')

INSERT INTO Sinhvien(MaSV,HotenSV,NgaySinh,MaLop) VALUES('218001',N'Bùi Chí Bảo','01/01/2003','001')
INSERT INTO Sinhvien(MaSV,HotenSV,NgaySinh,MaLop) VALUES('218002',N'Nguyễn Lâm Sơn','09/22/2003','001')
INSERT INTO Sinhvien(MaSV,HotenSV,NgaySinh,MaLop) VALUES('218003',N'Trần Thị C','12/11/2003','002')
INSERT INTO Sinhvien(MaSV,HotenSV,NgaySinh,MaLop) VALUES('218004',N'Bùi Văn D','04/02/2003','002')


