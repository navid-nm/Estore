CREATE TABLE IF NOT EXISTS Items (
     id INTEGER AUTO_INCREMENT PRIMARY KEY,
     name TEXT NOT NULL,
     description TEXT NOT NULL,
     startingprice DECIMAL(9, 2) NOT NULL,
     currentprice DECIMAL(9, 2) NOT NULL,
     itemcondition TEXT NOT NULL,
     username TEXT NOT NULL,
     views INTEGER NOT NULL,
     datelisted DATETIME NOT NULL,
     conclusiondate DATETIME NOT NULL,
     concluded BOOLEAN NOT NULL
);

CREATE TABLE IF NOT EXISTS Bids (
    id INTEGER AUTO_INCREMENT PRIMARY KEY,
    amount DECIMAL(9, 2) NOT NULL,
    datemade DATETIME NOT NULL,
    username TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS ItemHasBids (
    itemid INTEGER,
    bidid INTEGER,
    FOREIGN KEY (itemid) REFERENCES Items(id),
    FOREIGN KEY (bidid) REFERENCES Bids(id)
);