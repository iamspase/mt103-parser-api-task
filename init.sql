CREATE TABLE IF NOT EXISTS MT103Messages (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,

    ReferenceNumber TEXT NOT NULL,
    BankOperationCode TEXT NOT NULL,

    ValueDate TEXT NOT NULL, -- store as ISO string (yyyy-MM-dd)

    Currency TEXT NOT NULL,
    InterbankSettled REAL NOT NULL,

    OrderingCustomer TEXT,
    AccountWithInstitution TEXT,
    Recepient TEXT,
    RemittanceInformation TEXT,

    DetailsOfCharge TEXT
);