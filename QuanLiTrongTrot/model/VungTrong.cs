using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLiTrongTrot.Model
{
    /// <summary>
    /// Model class cho bảng VungTrong - Vùng trồng
    /// </summary>
    public class VungTrong
    {
        #region Properties

        public int Id { get; set; }
        public double QuyMo { get; set; }      // FLOAT trong SQL -> double trong C#
        public string DiaChi { get; set; }     // NVARCHAR(50)
        public int BanDoId { get; set; }       // Foreign key đến BanDoPhanBo

        #endregion

        #region Constructors

        public VungTrong() { }

        public VungTrong(double quyMo, string diaChi, int banDoId)
        {
            QuyMo = quyMo;
            DiaChi = diaChi;
            BanDoId = banDoId;
        }

        #endregion

        #region Static Methods - CRUD Operations

        /// <summary>
        /// Lấy tất cả VungTrong với STT tự động
        /// </summary>
        public static DataTable GetAll()
        {
            string query = @"
                SELECT 
                    ROW_NUMBER() OVER (ORDER BY Id) AS STT,
                    Id, QuyMo, DiaChi, BanDoId 
                FROM VungTrong";
            return DataProvider.ExecuteQuery(query);
        }

        /// <summary>
        /// Lấy ID nhỏ nhất còn trống hoặc ID tiếp theo
        /// </summary>
        public static int GetNextId()
        {
            // Tìm ID nhỏ nhất còn trống (gap)
            string gapQuery = @"
                SELECT TOP 1 t1.Id + 1 AS NextId
                FROM VungTrong t1
                LEFT JOIN VungTrong t2 ON t1.Id + 1 = t2.Id
                WHERE t2.Id IS NULL
                ORDER BY t1.Id";

            int gapId = DataProvider.ExecuteScalar(gapQuery);
            if (gapId > 0)
                return gapId;

            // Kiểm tra bảng rỗng
            int maxId = DataProvider.ExecuteScalar("SELECT ISNULL(MAX(Id), 0) FROM VungTrong");
            if (maxId == 0)
                return 1;

            // Kiểm tra ID = 1 có tồn tại không
            int hasId1 = DataProvider.ExecuteScalar("SELECT COUNT(*) FROM VungTrong WHERE Id = 1");
            if (hasId1 == 0)
                return 1;

            return maxId + 1;
        }

        /// <summary>
        /// Thêm VungTrong mới với ID tự động lấp gap
        /// </summary>
        public int Insert()
        {
            Id = GetNextId();

            string query = @"
                SET IDENTITY_INSERT VungTrong ON;
                INSERT INTO VungTrong (Id, QuyMo, DiaChi, BanDoId) 
                VALUES (@Id, @QuyMo, @DiaChi, @BanDoId);
                SET IDENTITY_INSERT VungTrong OFF;";

            return DataProvider.ExecuteNonQuery(query,
                new SqlParameter("@Id", Id),
                new SqlParameter("@QuyMo", QuyMo),
                new SqlParameter("@DiaChi", DiaChi),
                new SqlParameter("@BanDoId", BanDoId));
        }

        /// <summary>
        /// Cập nhật VungTrong
        /// </summary>
        public int Update()
        {
            string query = @"
                UPDATE VungTrong 
                SET QuyMo = @QuyMo, DiaChi = @DiaChi, BanDoId = @BanDoId 
                WHERE Id = @Id";

            return DataProvider.ExecuteNonQuery(query,
                new SqlParameter("@Id", Id),
                new SqlParameter("@QuyMo", QuyMo),
                new SqlParameter("@DiaChi", DiaChi),
                new SqlParameter("@BanDoId", BanDoId));
        }

        /// <summary>
        /// Xóa VungTrong và tất cả dữ liệu liên quan (cascade)
        /// </summary>
        public static int DeleteCascade(int vungTrongId)
        {
            using (var connection = new SqlConnection(DataProvider.connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Cấp 3: Bảng trung gian
                        ExecuteInTransaction(connection, transaction,
                            "DELETE FROM PhanBon_CoSoBan WHERE PhanBonId IN (SELECT Id FROM PhanBon WHERE VTId = @VTId)", vungTrongId);
                        ExecuteInTransaction(connection, transaction,
                            "DELETE FROM PhanBon_CoSoSanXuat WHERE PhanBonId IN (SELECT Id FROM PhanBon WHERE VTId = @VTId)", vungTrongId);
                        ExecuteInTransaction(connection, transaction,
                            "DELETE FROM ThuocBVTV_CoSoBan WHERE ThuocBVTVId IN (SELECT Id FROM ThuocBVTV WHERE VTId = @VTId)", vungTrongId);
                        ExecuteInTransaction(connection, transaction,
                            "DELETE FROM ThuocBVTV_CoSoSanXuat WHERE ThuocBVTVId IN (SELECT Id FROM ThuocBVTV WHERE VTId = @VTId)", vungTrongId);

                        // Cấp 2: Phụ thuộc SinhVatGayHai
                        ExecuteInTransaction(connection, transaction,
                            "DELETE FROM CapNhat_SVGH WHERE SVId IN (SELECT Id FROM SinhVatGayHai WHERE VTId = @VTId)", vungTrongId);
                        ExecuteInTransaction(connection, transaction,
                            "DELETE FROM TuoiSau WHERE SVId IN (SELECT Id FROM SinhVatGayHai WHERE VTId = @VTId)", vungTrongId);

                        // Cấp 1: Phụ thuộc trực tiếp VungTrong
                        ExecuteInTransaction(connection, transaction, "DELETE FROM SinhVatGayHai WHERE VTId = @VTId", vungTrongId);
                        ExecuteInTransaction(connection, transaction, "DELETE FROM PhanBon WHERE VTId = @VTId", vungTrongId);
                        ExecuteInTransaction(connection, transaction, "DELETE FROM ThuocBVTV WHERE VTId = @VTId", vungTrongId);
                        ExecuteInTransaction(connection, transaction, "DELETE FROM GiongCayDauDong WHERE VTId = @VTId", vungTrongId);

                        // Cuối cùng: Xóa VungTrong
                        ExecuteInTransaction(connection, transaction, "DELETE FROM VungTrong WHERE Id = @VTId", vungTrongId);

                        transaction.Commit();
                        return 1;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Private Helper Methods

        private static void ExecuteInTransaction(SqlConnection connection, SqlTransaction transaction, string query, int vungTrongId)
        {
            try
            {
                using (var command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.AddWithValue("@VTId", vungTrongId);
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                // Bỏ qua nếu bảng không tồn tại
                if (!ex.Message.Contains("Invalid object name"))
                    throw;
            }
        }

        #endregion
    }
}
