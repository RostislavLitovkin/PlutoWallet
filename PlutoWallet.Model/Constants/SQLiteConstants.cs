using SQLite;

namespace PlutoWallet.Model.Constants
{
    /// <summary>
    /// Source: https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/database-sqlite?view=net-maui-8.0
    /// </summary>
    public static class SQLiteConstants
    {
        public const string NftDatabaseFilename = "NftSQLite.db3";
        public const string CollectionDatabaseFilename = "CollectionSQLite.db3";


        public const SQLiteOpenFlags NftDatabaseFlags =
            // open the database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache |

            SQLiteOpenFlags.ProtectionNone;

        public const SQLiteOpenFlags CollectionDatabaseFlags =
            // open the database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache |

            SQLiteOpenFlags.ProtectionNone;

        public static string NftDatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, NftDatabaseFilename);
        public static string CollectionDatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, CollectionDatabaseFilename);
    }
}
