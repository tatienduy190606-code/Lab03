using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab03_OOPAdvanced;

public class Nguoi
{
    public string HoTen { get; set; }
    public DateTime NgaySinh { get; set; }

    public Nguoi(string hoTen, DateTime ngaySinh)
    {
        HoTen = hoTen;
        NgaySinh = ngaySinh;
    }

    public virtual string MoTa() =>
        $"{HoTen} - {NgaySinh:dd/MM/yyyy}";
}

public class SinhVien : Nguoi, IValidatable, IPrintable
{
    public string MaSV { get; set; }
    public double DiemTB { get; set; }

    public SinhVien(string ma, string ten, DateTime ngaySinh, double diem)
        : base(ten, ngaySinh)
    {
        MaSV = ma;
        DiemTB = diem;
    }

    public override string MoTa() =>
        base.MoTa() + $" - {MaSV} - {DiemTB:N2}";

    public bool IsValid(out string message)
    {
        if (string.IsNullOrWhiteSpace(MaSV)) { message = "Thiếu mã"; return false; }
        if (DiemTB < 0 || DiemTB > 10) { message = "Điểm sai"; return false; }
        message = "";
        return true;
    }

    public string ToDisplayText() => $"{MaSV} - {HoTen}";
}

public abstract class NguoiHoc
{
    public string HoTen { get; set; } = "";
    public int SoTinChi { get; set; }
    public abstract decimal TinhHocPhi();
}

public class SinhVienChinhQuy : NguoiHoc
{
    public override decimal TinhHocPhi() => SoTinChi * 450000m;
}

public class SinhVienLienThong : NguoiHoc
{
    public override decimal TinhHocPhi() => SoTinChi * 520000m;
}

public interface IValidatable
{
    bool IsValid(out string message);
}

public interface IPrintable
{
    string ToDisplayText();
}

// Bài 6: Hệ thống nhân viên
public abstract class NhanVien
{
    public string HoTen { get; set; } = "";
    public decimal LuongCoBan { get; set; }
    public abstract decimal TinhLuong();
    public virtual string MoTa() => HoTen;
}

public class NhanVienVanPhong : NhanVien
{
    public int SoNgayCong { get; set; }
    public override decimal TinhLuong() => LuongCoBan * SoNgayCong / 26m;
    public override string MoTa() => $"{HoTen} - Văn phòng - Lương: {TinhLuong():N0}";
}

public class NhanVienKinhDoanh : NhanVien
{
    public decimal DoanhSo { get; set; }
    public decimal TyLeHoaHong { get; set; } = 0.05m;
    public override decimal TinhLuong() => LuongCoBan + DoanhSo * TyLeHoaHong;
    public override string MoTa() => $"{HoTen} - Kinh doanh - Lương: {TinhLuong():N0}";
}

public class GiangVien : NhanVien
{
    public int SoTietDay { get; set; }
    public decimal PhuCapMoiTiet { get; set; } = 150000m;
    public override decimal TinhLuong() => LuongCoBan + SoTietDay * PhuCapMoiTiet;
    public override string MoTa() => $"{HoTen} - Giảng viên - Lương: {TinhLuong():N0}";
}

// Bài 7: Quản lý phương tiện
public interface IMove
{
    double TocDoToiDa { get; }
    decimal ChiPhiVanHanh(double km);
    string Move();
}

public class XeMay : IMove
{
    public double TocDoToiDa => 90;
    public decimal ChiPhiVanHanh(double km) => (decimal)km * 1800m / 100m;
    public string Move() => "Xe máy đang di chuyển";
}

public class OTo : IMove
{
    public double TocDoToiDa => 180;
    public decimal ChiPhiVanHanh(double km) => (decimal)km * 9000m / 100m;
    public string Move() => "Ô tô đang di chuyển";
}

public class XeDap : IMove
{
    public double TocDoToiDa => 30;
    public decimal ChiPhiVanHanh(double km) => 0;
    public string Move() => "Xe đạp đang di chuyển";
}

// Bài 8: Quản lý tài liệu thư viện
public abstract class TaiLieu
{
    public string MaTL { get; set; } = "";
    public string TieuDe { get; set; } = "";
    public abstract string Loai { get; }
    public virtual string ThongTin() => $"{MaTL} - {TieuDe} - {Loai}";
}

public class Sach : TaiLieu
{
    public string TacGia { get; set; } = "";
    public override string Loai => "Sách";
    public override string ThongTin() => base.ThongTin() + $" - Tác giả: {TacGia}";
}

public class TapChi : TaiLieu
{
    public int SoPhatHanh { get; set; }
    public override string Loai => "Tạp chí";
    public override string ThongTin() => base.ThongTin() + $" - Số: {SoPhatHanh}";
}

public class LuanVan : TaiLieu
{
    public string ChuyenNganh { get; set; } = "";
    public override string Loai => "Luận văn";
    public override string ThongTin() => base.ThongTin() + $" - Chuyên ngành: {ChuyenNganh}";
}

// Bài 9: Hệ thống thanh toán
public interface IPayment
{
    bool Validate(out string message);
    bool Pay(decimal amount);
    string GetInfo();
}

public class ThanhToanTienMat : IPayment
{
    public decimal SoTien { get; set; }
    public bool Validate(out string message)
    {
        if (SoTien <= 0) { message = "Số tiền phải lớn hơn 0"; return false; }
        message = "Hợp lệ"; return true;
    }
    public bool Pay(decimal amount) { SoTien = amount; return Validate(out _); }
    public string GetInfo() => $"Thanh toán tiền mặt: {SoTien:N0} VNĐ";
}

public class ChuyenKhoan : IPayment
{
    public string SoTaiKhoan { get; set; } = "";
    public bool Validate(out string message)
    {
        if (string.IsNullOrWhiteSpace(SoTaiKhoan)) { message = "Thiếu số tài khoản"; return false; }
        message = "Hợp lệ"; return true;
    }
    public bool Pay(decimal amount) => Validate(out _);
    public string GetInfo() => $"Chuyển khoản: TK {SoTaiKhoan}";
}

public class ViDienTu : IPayment
{
    public string SoVi { get; set; } = "";
    public bool Validate(out string message)
    {
        if (string.IsNullOrWhiteSpace(SoVi)) { message = "Thiếu số ví"; return false; }
        message = "Hợp lệ"; return true;
    }
    public bool Pay(decimal amount) => Validate(out _);
    public string GetInfo() => $"Ví điện tử: {SoVi}";
}

// Bài 10: Nâng cấp quản lý sinh viên
public class SinhVienNangCao : IPrintable
{
    public string MaSV { get; set; } = "";
    public string HoTen { get; set; } = "";
    public string LoaiSV { get; set; } = "";
    public double DiemTB { get; set; }
    public string ToDisplayText() => $"{MaSV} - {HoTen} - {LoaiSV} - ĐTB: {DiemTB:N2}";
}

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("===== LAB 3 - KE THUA, DA HINH, ABSTRACT, INTERFACE =====\n");

        Bai1Den5();
        Bai6();
        Bai7();
        Bai8();
        Bai9();
        Bai10();

        Console.WriteLine("\nHoàn thành Lab 3.");
    }

    static void Bai1Den5()
    {
        Console.WriteLine("1-5. Kế thừa, override, abstract, interface và đa hình");

        var sv = new SinhVien("SV001", "Nguyễn Văn An", new DateTime(2005, 5, 10), 8.5);
        Console.WriteLine("MoTa: " + sv.MoTa());
        Console.WriteLine($"Valid: {sv.IsValid(out var msg)} {msg}");
        Console.WriteLine("Display: " + sv.ToDisplayText());

        var ds = new List<NguoiHoc>
        {
            new SinhVienChinhQuy { HoTen = "An", SoTinChi = 18 },
            new SinhVienLienThong { HoTen = "Bình", SoTinChi = 15 }
        };

        decimal tong = 0;
        foreach (var item in ds)
        {
            var hocPhi = item.TinhHocPhi();
            tong += hocPhi;
            Console.WriteLine($"{item.HoTen}: {hocPhi:N0} VNĐ");
        }
        Console.WriteLine($"Tổng học phí: {tong:N0} VNĐ\n");
    }

    static void Bai6()
    {
        Console.WriteLine("6. Hệ thống nhân viên");
        var ds = new List<NhanVien>
        {
            new NhanVienVanPhong { HoTen = "Nguyễn An", LuongCoBan = 12000000, SoNgayCong = 26 },
            new NhanVienKinhDoanh { HoTen = "Trần Bình", LuongCoBan = 8000000, DoanhSo = 100000000 },
            new GiangVien { HoTen = "Lê Cường", LuongCoBan = 10000000, SoTietDay = 30 }
        };
        foreach (var nv in ds) Console.WriteLine(nv.MoTa());
        Console.WriteLine();
    }

    static void Bai7()
    {
        Console.WriteLine("7. Quản lý phương tiện");
        var ds = new List<IMove> { new XeMay(), new OTo(), new XeDap() };
        foreach (var p in ds)
            Console.WriteLine($"{p.Move()} | Tốc độ tối đa: {p.TocDoToiDa} km/h | Chi phí 100 km: {p.ChiPhiVanHanh(100):N0} VNĐ");
        Console.WriteLine();
    }

    static void Bai8()
    {
        Console.WriteLine("8. Quản lý tài liệu thư viện");
        var ds = new List<TaiLieu>
        {
            new Sach { MaTL = "S01", TieuDe = "Lập trình C#", TacGia = "Nguyễn A" },
            new TapChi { MaTL = "TC01", TieuDe = "CNTT hiện đại", SoPhatHanh = 12 },
            new LuanVan { MaTL = "LV01", TieuDe = "Ứng dụng OOP", ChuyenNganh = "Công nghệ phần mềm" }
        };
        string tuKhoa = "C#";
        Console.WriteLine($"Tìm kiếm từ khóa: {tuKhoa}");
        foreach (var tl in ds.Where(x => x.TieuDe.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)))
            Console.WriteLine(tl.ThongTin());
        Console.WriteLine("Toàn bộ tài liệu:");
        foreach (var tl in ds) Console.WriteLine(tl.ThongTin());
        Console.WriteLine();
    }

    static void Bai9()
    {
        Console.WriteLine("9. Hệ thống thanh toán");
        var payments = new List<IPayment>
        {
            new ThanhToanTienMat(),
            new ChuyenKhoan { SoTaiKhoan = "0123456789" },
            new ViDienTu { SoVi = "VNPAY-001" }
        };
        foreach (var p in payments)
        {
            p.Pay(500000);
            Console.WriteLine(p.GetInfo());
        }
        Console.WriteLine();
    }

    static void Bai10()
    {
        Console.WriteLine("10. Nâng cấp quản lý sinh viên");
        var ds = new List<SinhVienNangCao>
        {
            new() { MaSV = "SV001", HoTen = "An", LoaiSV = "Chính quy", DiemTB = 8.5 },
            new() { MaSV = "SV002", HoTen = "Bình", LoaiSV = "Liên thông", DiemTB = 7.2 },
            new() { MaSV = "SV003", HoTen = "Cường", LoaiSV = "Chính quy", DiemTB = 9.1 }
        };

        Console.WriteLine("Danh sách:");
        foreach (var sv in ds) Console.WriteLine(sv.ToDisplayText());

        Console.WriteLine("\nThống kê theo loại:");
        foreach (var group in ds.GroupBy(x => x.LoaiSV))
            Console.WriteLine($"{group.Key}: {group.Count()} sinh viên, ĐTB = {group.Average(x => x.DiemTB):N2}");
    }
}
