
-- Table Checks
SELECT * FROM Publishers
SELECT * FROM Genres
SELECT * FROM Games

-- Deleting Everything from Table
DELETE FROM Games;
DELETE FROM Publishers;
DELETE FROM Genres;

-- Reset ID to 0
DBCC CHECKIDENT ('Genres', RESEED, 0);
DBCC CHECKIDENT ('Games', RESEED, 0);
DBCC CHECKIDENT ('Publishers', RESEED, 0);



-- SEEDING DATA (Uncomment Commit if happy of Data going in)
BEGIN TRANSACTION

INSERT INTO Publishers (Name, CreatedDateUtc)
VALUES
    ('Saber Interactive',GETDATE()),
    ('Bandai Namco Entertainment', GETDATE()),
    ('Capcom', GETDATE()),
    ('Electronic Arts', GETDATE()),
    ('Konami', GETDATE()),
    ('Koei Tecmo', GETDATE()),
    ('Microsoft', GETDATE()),
    ('Sega', GETDATE()),
    ('Square Enix', GETDATE()),
    ('Ubisoft', GETDATE()),
    ('Warner Bros. Games', GETDATE()),
    ('Sony Interactive Entertainment', GETDATE()),
    ('Bethesda Softworks', GETDATE()),
    ('Activision', GETDATE()),
    ('Blizzard Entertainment', GETDATE()),
    ('Take-Two Interactive', GETDATE()),
    ('2K', GETDATE()),
    ('Deep Silver', GETDATE()),
    ('THQ Nordic', GETDATE()),
    ('Focus Entertainment', GETDATE()),
    ('Paradox Interactive', GETDATE()),
    ('Devolver Digital', GETDATE()),
    ('Annapurna Interactive', GETDATE()),
    ('Team17', GETDATE()),
    ('Nacon', GETDATE()),
    ('Private Division', GETDATE()),
    ('Gearbox Publishing', GETDATE()),
    ('Riot Games', GETDATE()),
    ('Epic Games', GETDATE()),
    ('Krafton', GETDATE()),
    ('CD Projekt', GETDATE()),
    ('Larian Studios', GETDATE()),
    ('Mojang Studios', GETDATE()),
    ('Valve', GETDATE()),
    ('Atlus', GETDATE()),
    ('Marvelous', GETDATE()),
    ('NIS America', GETDATE()),
    ('XSEED Games', GETDATE()),
    ('Idea Factory', GETDATE()),
    ('Compile Heart', GETDATE()),
    ('Nintendo', GETDATE()),
    ('FromSoftware', GETDATE());

INSERT INTO Genres (Name)
VALUES
    ('Action'),
    ('Adventure'),
    ('Role-Playing'),
    ('Strategy'),
    ('Simulation'),
    ('Sports'),
    ('Racing'),
    ('Puzzle'),
    ('Fighting'),
    ('Platformer'),
    ('Shooter'),
    ('Horror'),
    ('Other'),
    ('MMORPG'),
    ('JRPG'),
    ('Action RPG'),
    ('Survival Horror'),
    ('Sandbox'),
    ('Visual Novel'),
    ('Battle Royale'),
    ('MOBA'),
    ('Grand Strategy');


INSERT INTO Games (Name, ReleaseYear, CreatedDateUtc, GenreId, PublisherId)
VALUES
    ('Elden Ring', 2022, GETDATE(), 16, 41),
    ('Dark Souls', 2011, GETDATE(), 16, 41),
    ('Dark Souls II', 2014, GETDATE(), 16, 41),
    ('Dark Souls III', 2016, GETDATE(), 16, 41),
    ('Sekiro: Shadows Die Twice', 2019, GETDATE(), 1, 41),
    ('Bloodborne', 2015, GETDATE(), 16, 11),
    ('Armored Core VI: Fires of Rubicon', 2023, GETDATE(), 1, 41),

    ('The Legend of Zelda: Breath of the Wild', 2017, GETDATE(), 2, 40),
    ('The Legend of Zelda: Tears of the Kingdom', 2023, GETDATE(), 2, 40),
    ('Super Mario Odyssey', 2017, GETDATE(), 10, 40),
    ('Super Mario Bros. Wonder', 2023, GETDATE(), 10, 40),
    ('Mario Kart 8 Deluxe', 2017, GETDATE(), 7, 40),
    ('Animal Crossing: New Horizons', 2020, GETDATE(), 5, 40),
    ('Super Smash Bros. Ultimate', 2018, GETDATE(), 9, 40),
    ('Metroid Dread', 2021, GETDATE(), 1, 40),
    ('Pokémon Legends: Arceus', 2022, GETDATE(), 16, 40),
    ('Fire Emblem: Three Houses', 2019, GETDATE(), 3, 40),
    ('Pikmin 4', 2023, GETDATE(), 4, 40),
    ('Xenoblade Chronicles 3', 2022, GETDATE(), 15, 40),
    ('The Legend of Zelda: Ocarina of Time', 1998, GETDATE(), 2, 40),

    ('Tekken 8', 2024, GETDATE(), 9, 1),
    ('Tekken 7', 2015, GETDATE(), 9, 1),
    ('Dark Souls Remastered', 2018, GETDATE(), 16, 1),
    ('Dragon Ball FighterZ', 2018, GETDATE(), 9, 1),
    ('Dragon Ball Z: Kakarot', 2020, GETDATE(), 16, 1),
    ('Little Nightmares', 2017, GETDATE(), 12, 1),

    ('Resident Evil 4', 2023, GETDATE(), 17, 2),
    ('Resident Evil Village', 2021, GETDATE(), 17, 2),
    ('Resident Evil 2', 2019, GETDATE(), 17, 2),
    ('Monster Hunter: World', 2018, GETDATE(), 16, 2),
    ('Monster Hunter Rise', 2021, GETDATE(), 16, 2),
    ('Street Fighter 6', 2023, GETDATE(), 9, 2),
    ('Devil May Cry 5', 2019, GETDATE(), 1, 2),
    ('Dragon''s Dogma 2', 2024, GETDATE(), 16, 2),

    ('The Sims 4', 2014, GETDATE(), 5, 3),
    ('Mass Effect 2', 2010, GETDATE(), 16, 3),
    ('Mass Effect 3', 2012, GETDATE(), 16, 3),
    ('Dragon Age: Inquisition', 2014, GETDATE(), 3, 3),
    ('Dead Space', 2023, GETDATE(), 12, 3),
    ('Star Wars Jedi: Fallen Order', 2019, GETDATE(), 1, 3),
    ('Star Wars Jedi: Survivor', 2023, GETDATE(), 1, 3),
    ('It Takes Two', 2021, GETDATE(), 10, 3),

    ('Metal Gear Solid V: The Phantom Pain', 2015, GETDATE(), 1, 4),
    ('Metal Gear Solid 3: Snake Eater', 2004, GETDATE(), 1, 4),
    ('Silent Hill 2', 2024, GETDATE(), 12, 4),
    ('Castlevania: Symphony of the Night', 1997, GETDATE(), 10, 4),

    ('Nioh', 2017, GETDATE(), 16, 5),
    ('Nioh 2', 2020, GETDATE(), 16, 5),
    ('Wo Long: Fallen Dynasty', 2023, GETDATE(), 16, 5),

    ('Halo: Combat Evolved', 2001, GETDATE(), 11, 6),
    ('Halo 2', 2004, GETDATE(), 11, 6),
    ('Halo Infinite', 2021, GETDATE(), 11, 6),
    ('Forza Horizon 5', 2021, GETDATE(), 7, 6),
    ('Gears 5', 2019, GETDATE(), 11, 6),
    ('Microsoft Flight Simulator', 2020, GETDATE(), 5, 6),

    ('Persona 5 Royal', 2019, GETDATE(), 15, 34),
    ('Persona 4 Golden', 2012, GETDATE(), 15, 34),
    ('Metaphor: ReFantazio', 2024, GETDATE(), 15, 34),

    ('Yakuza 0', 2015, GETDATE(), 1, 7),
    ('Yakuza: Like a Dragon', 2020, GETDATE(), 15, 7),
    ('Like a Dragon: Infinite Wealth', 2024, GETDATE(), 15, 7),
    ('Sonic Frontiers', 2022, GETDATE(), 10, 7),
    ('Persona 5 Strikers', 2020, GETDATE(), 1, 7),

    ('Final Fantasy VII Remake', 2020, GETDATE(), 15, 8),
    ('Final Fantasy VII Rebirth', 2024, GETDATE(), 15, 8),
    ('Final Fantasy XVI', 2023, GETDATE(), 16, 8),
    ('Final Fantasy XV', 2016, GETDATE(), 16, 8),
    ('Kingdom Hearts III', 2019, GETDATE(), 16, 8),
    ('Dragon Quest XI', 2017, GETDATE(), 15, 8),

    ('Assassin''s Creed II', 2009, GETDATE(), 1, 9),
    ('Assassin''s Creed Odyssey', 2018, GETDATE(), 16, 9),
    ('Far Cry 5', 2018, GETDATE(), 11, 9),
    ('Far Cry 6', 2021, GETDATE(), 11, 9),
    ('Watch Dogs 2', 2016, GETDATE(), 1, 9),
    ('Prince of Persia: The Lost Crown', 2024, GETDATE(), 10, 9),

    ('The Witcher 3: Wild Hunt', 2015, GETDATE(), 16, 30),
    ('Cyberpunk 2077', 2020, GETDATE(), 16, 30),

    ('Baldur''s Gate 3', 2023, GETDATE(), 3, 31),
    ('Divinity: Original Sin 2', 2017, GETDATE(), 3, 31),

    ('Minecraft', 2011, GETDATE(), 18, 32),
    ('Minecraft Dungeons', 2020, GETDATE(), 16, 32),

    ('Half-Life 2', 2004, GETDATE(), 11, 33),
    ('Portal 2', 2011, GETDATE(), 8, 33),
    ('Counter-Strike 2', 2023, GETDATE(), 11, 33),
    ('Left 4 Dead 2', 2009, GETDATE(), 11, 33),
    ('Dota 2', 2013, GETDATE(), 21, 33),

    ('Diablo IV', 2023, GETDATE(), 16, 14),
    ('Overwatch 2', 2022, GETDATE(), 11, 14),
    ('StarCraft II', 2010, GETDATE(), 4, 14),

    ('The Elder Scrolls V: Skyrim', 2011, GETDATE(), 3, 12),
    ('Fallout 4', 2015, GETDATE(), 3, 12),
    ('Starfield', 2023, GETDATE(), 3, 12),
    ('DOOM Eternal', 2020, GETDATE(), 11, 12),

    ('Grand Theft Auto V', 2013, GETDATE(), 1, 15),
    ('Red Dead Redemption 2', 2018, GETDATE(), 1, 15),
    ('Red Dead Redemption', 2010, GETDATE(), 1, 15),

    ('Borderlands 3', 2019, GETDATE(), 11, 26),
    ('Tiny Tina''s Wonderlands', 2022, GETDATE(), 16, 26),

    ('Hades', 2020, GETDATE(), 1, 22),
    ('Stray', 2022, GETDATE(), 2, 22),

    ('The Outer Worlds', 2019, GETDATE(), 3, 25),
    ('Kerbal Space Program', 2015, GETDATE(), 5, 25),

    ('Cities: Skylines', 2015, GETDATE(), 5, 20),
    ('Crusader Kings III', 2020, GETDATE(), 22, 20),

    ('Total War: Warhammer III', 2022, GETDATE(), 4, 18),
    ('Destroy All Humans!', 2020, GETDATE(), 1, 18),

    ('Dead Island 2', 2023, GETDATE(), 1, 17),
    ('Metro Exodus', 2019, GETDATE(), 11, 17),

    ('The Escapists', 2015, GETDATE(), 4, 23),
    ('Worms W.M.D', 2016, GETDATE(), 4, 23),

    ('Disco Elysium', 2019, GETDATE(), 3, 21),
    ('Fall Guys', 2020, GETDATE(), 10, 28),
    ('League of Legends', 2009, GETDATE(), 21, 27),
    ('PUBG: Battlegrounds', 2017, GETDATE(), 20, 29);

    -- ROLLBACK
    -- COMMIT