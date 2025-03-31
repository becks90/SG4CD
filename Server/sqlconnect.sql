-- phpMyAdmin SQL Dump
-- version 5.1.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Erstellungszeit: 31. Mrz 2025 um 18:54
-- Server-Version: 5.7.24
-- PHP-Version: 8.3.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `sqlconnect`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `advisedperson`
--

CREATE TABLE `advisedperson` (
  `id` int(11) NOT NULL,
  `userid` int(11) NOT NULL,
  `advisorid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `advisedperson`
--

INSERT INTO `advisedperson` (`id`, `userid`, `advisorid`) VALUES
(1, 10, 16),
(2, 12, 16),
(3, 17, 16);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `answercategories`
--

CREATE TABLE `answercategories` (
  `id` int(10) NOT NULL,
  `name` varchar(64) NOT NULL,
  `defaultPoints` int(8) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `answercategories`
--

INSERT INTO `answercategories` (`id`, `name`, `defaultPoints`) VALUES
(1, 'Akzeptanz', 3),
(2, 'Vermeidung', 0),
(3, 'Problemlösung', 5),
(4, 'Kognitive Umbewertung', 5),
(5, 'Rumination', 0),
(6, 'Unterdrückung des Gefühlsausdrucks', 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `diary`
--

CREATE TABLE `diary` (
  `id` int(11) NOT NULL,
  `userid` int(11) NOT NULL,
  `entrydate` datetime NOT NULL,
  `feeling` int(4) NOT NULL,
  `entry` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `diary`
--

INSERT INTO `diary` (`id`, `userid`, `entrydate`, `feeling`, `entry`) VALUES
(9, 10, '2025-03-02 12:46:18', 4, 'Ich hatte heute Streit mit meinem Freund'),
(10, 12, '2025-03-21 13:26:18', 1, 'Ich fühle mich heute toll, das Wetter ist schön und das Wochenende ist da! Ich freue mich nachher meinen Freund zu treffen, wir gehen auf den Bolzplatz ein wenig kicken.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n'),
(11, 12, '2025-03-22 13:46:44', 4, 'Nerv nicht, keine Zeit'),
(12, 12, '2025-03-23 21:21:33', 4, 'Wieder keine Zeit, sorry'),
(14, 17, '2025-03-24 19:22:30', 2, 'Bin nur am testen, danke'),
(15, 17, '2025-03-25 13:25:55', 1, 'Spitze'),
(16, 17, '2025-01-05 00:00:00', 1, 'Heute war ein wunderschöner Tag! Ich hatte so viel Spaß im Park, habe mit Freunden gespielt und bin viel gelaufen!'),
(17, 17, '2025-02-11 00:00:00', 2, 'Der Tag war okay. Ich habe mit meiner Schwester ein Puzzle gemacht, aber irgendwie war es nicht so spannend.'),
(18, 17, '2025-03-14 00:00:00', 3, 'Es war ein normaler Tag. Ich habe meine Hausaufgaben gemacht und dann ein wenig Fernsehen geschaut.'),
(19, 17, '2025-02-02 00:00:00', 4, 'Heute war ein trauriger Tag. Ich hatte Streit mit meinen Freunden und es hat mich wirklich verletzt.'),
(20, 17, '2025-02-09 00:00:00', 5, 'Es war ein ganz schrecklicher Tag. Ich habe mich ganz allein gefühlt, meine Eltern haben mich geschimpft und ich war sehr traurig.'),
(21, 17, '2025-03-07 00:00:00', 3, 'Der Tag war ganz okay. Ich habe mit meinen Eltern zusammen gekocht und danach eine kurze Runde im Garten gespielt.'),
(22, 17, '2025-02-12 00:00:00', 2, 'Es war ein mittelmäßiger Tag. Ich habe den ganzen Nachmittag mit meinen Cousins gespielt, aber ich war ein wenig müde.'),
(23, 17, '2025-02-15 00:00:00', 1, 'Heute war ein super toller Tag! Wir sind in den Zoo gegangen und haben viele Tiere gesehen, es war richtig spannend!'),
(24, 17, '2025-03-20 00:00:00', 4, 'Heute war ein eher schlechter Tag. Ich habe mich mit meinem besten Freund gestritten und wir haben uns nicht vertragen.'),
(25, 17, '2025-01-25 00:00:00', 5, 'Es war ein ganz furchtbarer Tag. Ich hatte einen riesigen Krach mit meinen Eltern und bin dann ganz traurig in mein Zimmer gegangen.'),
(26, 17, '2025-01-03 00:00:00', 3, 'Es war ein ruhiger Tag. Ich habe viel gelesen und danach ein wenig mit meinen Freunden draußen gespielt.'),
(27, 17, '2025-02-08 00:00:00', 1, 'Es war ein großartiger Tag! Wir haben einen Ausflug in die Berge gemacht und ich habe viel Spaß mit meiner Familie gehabt.'),
(28, 17, '2025-01-19 00:00:00', 2, 'Der Tag war ein bisschen langweilig. Ich habe den Nachmittag alleine verbracht und mich gelangweilt.'),
(29, 17, '2025-01-24 00:00:00', 3, 'Ich habe viel für die Schule gelernt und danach ein wenig auf dem Spielplatz gespielt. Es war ein normaler Tag.'),
(30, 17, '2025-01-30 00:00:00', 4, 'Es war ein ziemlich schlechter Tag. Meine Schwester hat mich geärgert und das hat mich wirklich genervt.'),
(31, 17, '2025-03-10 00:00:00', 5, 'Der Tag war schrecklich. Ich habe den ganzen Nachmittag geweint und niemand hat mit mir gespielt.'),
(32, 17, '2025-01-16 00:00:00', 2, 'Der Tag war okay. Ich habe mit meinem Hund gespielt, aber insgesamt war es nicht so aufregend.'),
(33, 17, '2025-02-22 00:00:00', 3, 'Es war ein durchschnittlicher Tag. Ich habe ein paar Freunde getroffen und wir haben ein bisschen Spaß gehabt.'),
(34, 17, '2025-02-17 00:00:00', 1, 'Ein fantastischer Tag! Ich habe ein Eis gegessen, mit meinen Freunden gespielt und alles war einfach nur super!'),
(35, 17, '2025-03-01 00:00:00', 4, 'Es war ein schlechter Tag. Ich habe mich den ganzen Tag alleine gefühlt und irgendwie war nichts gut.'),
(36, 17, '2025-03-05 00:00:00', 5, 'Es war ein wirklich furchtbarer Tag. Es gab so viele Streitereien zu Hause und ich fühlte mich schlecht.'),
(37, 17, '2025-01-13 00:00:00', 3, 'Der Tag war mittelmäßig. Ich habe viele Aufgaben erledigt und war danach ein wenig erschöpft.'),
(38, 17, '2025-01-21 00:00:00', 1, 'Es war ein toller Tag! Wir haben einen Ausflug in den Freizeitpark gemacht und ich habe viel gelacht.'),
(39, 17, '2025-02-04 00:00:00', 2, 'Der Tag war okay, aber ich habe den ganzen Nachmittag vor dem Fernseher verbracht und mich gelangweilt.'),
(40, 17, '2025-01-15 00:00:00', 4, 'Es war ein trauriger Tag. Ich hatte einen Streit mit meinen Eltern und das hat mich wirklich verletzt.'),
(41, 17, '2025-01-02 00:00:00', 3, 'Der Tag war ganz okay. Ich habe meine Freunde getroffen und wir haben ein bisschen Basketball gespielt.'),
(42, 17, '2025-02-25 00:00:00', 5, 'Es war ein ganz schrecklicher Tag. Ich habe mich sehr schlecht gefühlt und wollte einfach nur alleine sein.'),
(43, 17, '2025-02-28 00:00:00', 1, 'Es war ein super schöner Tag! Ich habe meine Familie besucht und wir haben den ganzen Tag miteinander verbracht.'),
(44, 17, '2025-03-26 22:51:51', 4, 'Heute geht es mir nicht ganz so gut, ich bin ein bisschen kränkelnd, dafür darf ich aber ein bisschen länger an der Konsole spielen!'),
(46, 17, '2025-03-27 15:40:51', 4, 'Heute mussten ich wieder zur Schule, ich wäre lieber noch etwas im Urlaub geblieben...'),
(47, 17, '2025-03-28 12:45:57', 4, 'Langsam wird es spannend, am Montag steht eine große Schularbeit an und ich bin sehr angespannt, ich hoffe ich schaffe das, es hängt sehr viel davon ab...'),
(48, 17, '2025-03-30 00:19:29', 4, 'Ich habe Angst davor');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ercategories`
--

CREATE TABLE `ercategories` (
  `id` int(11) NOT NULL,
  `name` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `ercategories`
--

INSERT INTO `ercategories` (`id`, `name`) VALUES
(1, 'Freunde'),
(2, 'Familie'),
(3, 'Schule'),
(4, 'Sport');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `playedscenarios`
--

CREATE TABLE `playedscenarios` (
  `id` int(11) NOT NULL,
  `userId` int(10) NOT NULL,
  `scenarioId` int(10) NOT NULL,
  `answerId` int(10) NOT NULL,
  `answercategory` int(10) NOT NULL,
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `playedscenarios`
--

INSERT INTO `playedscenarios` (`id`, `userId`, `scenarioId`, `answerId`, `answercategory`, `date`) VALUES
(1, 10, 3, 4, 2, '2025-03-20 16:07:50'),
(2, 12, 3, 4, 2, '2025-03-20 16:17:53'),
(3, 10, 3, 4, 2, '2025-03-20 16:17:54'),
(4, 12, 3, 5, 5, '2025-03-20 16:18:00'),
(5, 10, 3, 4, 2, '2025-03-20 16:26:53'),
(6, 12, 3, 4, 2, '2025-03-20 16:28:38'),
(7, 10, 3, 4, 2, '2025-03-21 14:34:08'),
(8, 12, 3, 6, 6, '2025-03-22 15:56:48'),
(9, 17, 3, 1, 1, '2025-01-01 00:00:00'),
(10, 17, 3, 2, 2, '2025-01-05 00:00:00'),
(11, 17, 3, 3, 3, '2025-01-10 00:00:00'),
(12, 17, 3, 4, 4, '2025-01-15 00:00:00'),
(13, 17, 3, 5, 5, '2025-01-20 00:00:00'),
(14, 17, 3, 6, 6, '2025-01-25 00:00:00'),
(15, 17, 4, 7, 3, '2025-02-01 00:00:00'),
(16, 17, 4, 8, 2, '2025-02-05 00:00:00'),
(17, 17, 4, 9, 1, '2025-02-10 00:00:00'),
(18, 17, 5, 10, 1, '2025-02-15 00:00:00'),
(19, 17, 5, 11, 3, '2025-02-20 00:00:00'),
(20, 17, 5, 12, 4, '2025-02-25 00:00:00'),
(21, 17, 5, 13, 2, '2025-03-01 00:00:00'),
(22, 17, 5, 20, 5, '2025-03-05 00:00:00'),
(23, 17, 6, 21, 1, '2025-03-10 00:00:00'),
(24, 17, 6, 22, 3, '2025-03-15 00:00:00'),
(25, 17, 6, 23, 4, '2025-03-20 00:00:00'),
(26, 17, 6, 24, 2, '2025-03-25 00:00:00'),
(28, 17, 6, 24, 2, '2025-03-26 23:06:45'),
(29, 17, 3, 4, 2, '2025-03-27 15:42:08'),
(30, 17, 3, 3, 4, '2025-03-27 15:46:48'),
(42, 17, 5, 13, 2, '2025-03-28 13:33:28');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `scenarioanswers`
--

CREATE TABLE `scenarioanswers` (
  `id` int(11) NOT NULL,
  `scenarioId` int(10) NOT NULL,
  `answer` varchar(255) NOT NULL,
  `response` varchar(255) NOT NULL,
  `answercategory` int(10) NOT NULL,
  `points` smallint(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `scenarioanswers`
--

INSERT INTO `scenarioanswers` (`id`, `scenarioId`, `answer`, `response`, `answercategory`, `points`) VALUES
(1, 3, 'Schreibe ihm, dass es dir leid tut, dass ihr euch gestritten habt. Man kann nicht immer der gleichen Meinung sein, aber das ist okey!', 'Du hattest Recht, wir haben darüber gesprochen und haben das geklärt. Mir geht es gleich etwas besser :)', 1, 3),
(2, 3, 'Versucht doch euren Streitpunkt nochmal zu klären, vielleicht koennt ihr einen Kompromiss finden', 'Das war ein sehr guter Tipp, danke! Wir haben das geklärt und gemerkt und einen Kompromiss gefunden der für uns beide akzeptabel ist.', 2, 5),
(3, 3, 'Ihr solltet euch nochmal im ruhigen darüber unterhalten nachdem die Emotionen nun etwas runter sind. Eventuell können eure Eltern dem Gespräch noch beisitzen um zu vermitteln.', 'Das Gespräch hat mir sehr geholfen, wir konnten Missverständnisse aufklären und ich fühle mich erleichtert und viel befreiter, danke!', 4, 5),
(4, 3, 'Ach, vergiss den Streit und ihn einfach!', 'Na gut, so wirklich glücklich bin ich damit nicht, aber was soll ich sonst machen?', 2, 0),
(5, 3, 'Überleg nochmal was du alles falsch gemacht hast!', 'Ich habe ständig darüber nachgedacht, und konnte mich beim Sport nicht mal richtig konzentrieren. Ich befürchte ich habe unsere Freundschaft zerstört und fühle mich richtig mies.', 5, 0),
(6, 3, 'Denk einfach nicht mehr dran, und meide den Kontakt sodass der Streit nicht nochmals aufkommt.', 'Ich habe versucht nicht mehr dran zu denken und bin ihm die ganze Zeit aus dem Weg gegangen. Ich fühle mich deswegen den ganzen Tag schon unwohl und konnte mich nicht konzentrieren in der Schule.', 6, 0),
(7, 4, 'Gehe doch mal nach unten und rede mit ihnen, das klärt sich bestimmt.', 'Du hattest Recht, sie hatten nur eine kleine Meinungsverschiedenheit, die sie jetzt geklärt haben. Wir spielen jetzt eine Runde Monopoly. Danke für deine Hilfe!', 3, 5),
(8, 4, 'Setz die Kopfhörer auf und schon hörst du es nicht mehr, ganz einfach...', 'Das hat zwar funktioniert, aber ich muss trotzdem dran denken ... Ich bin traurig wenn ich weis, dass meine Eltern streiten', 2, 0),
(9, 4, 'Auch Eltern streiten sich mal, das kommt vor und muss man akzeptieren. Ich bin mir sicher, dass sie sich schon gleich wieder vertragen.', 'Du hattest Recht! Sie haben gemerkt, dass nur ein Missverständnis vorlag und alles geklärt.', 1, 3),
(10, 5, 'Das ist doch völlig okey, jeder macht mal Fehler!', 'Damit hast du wohl Recht!', 1, 3),
(11, 5, 'Sprich doch mal mit deiner Lehrerin, vielleicht kann sie dir helfen!', 'Sehr gute Idee, werde ich direkt morgen in der Schule machen!', 3, 5),
(12, 5, 'Das bedeutet doch gar nichts, das war bestimmt nur ein schlechter Tag. Bereite dich beim nächsten mal vielleicht noch besser darauf vor und dann wird das super!', 'Du hast recht, meine Lehrerin wird überrascht sein wie gut meine Hausaufgaben morgen sind, ich mache sogar die Fleißaufgaben!', 4, 10),
(13, 5, 'Ach, ist doch rum ... wirf sie in den Papiermüll und gut ist!', 'Ich weiß ja nicht, so werde ich doch nicht besser in der Schule, oder? :/', 2, 0),
(20, 5, 'Du solltest etwas darüber nachdenken, schlechte Leistungen in der Schule sind nicht gut. Überleg mal was das für die Zukunft bedeuten kann!', 'Da hast du wohl Recht, jetzt geht es mir noch schlechter...', 5, 0),
(21, 6, 'Mach dir kein Kopf, Fehler passieren. Auch die Profis machen ständig Fehler!', 'Das stimmt, und au0erdem hab ich mein bestes gegeben!', 1, 3),
(22, 6, 'Du könntest die Situation mit deinem Trainer üben, vielleicht hat er ein paar Tipps und Tricks für dich!', 'Das ist eine sehr gute Idee, er hat ja viel Erfahrung und kann mir bestimmt helfen noch besser zu werden.', 3, 5),
(23, 6, 'Alles gut, Fehler sind beim Sport doch ganz normal! Beim nächsten mal schießt du bestimmt das entscheidende Tor für euch!', 'Das stimmt, ich bin jetzt schon wieder motiviert und voller Vorfreude auf das nächste Spiel.', 4, 5),
(24, 6, 'Ohje, ich glaube an deiner Stelle würde ich nie wieder hingehen wollen ...', 'Ja, aber dabei spiele ich doch eigentlich sehr gerne...', 2, 0),
(25, 6, 'Tu vor den anderen und dem Trainer einfach so als würde es dich gar nicht jucken wenn sowas passiert, du musst den coolen spielen!', 'Hm, okay. Aber innerlich geht es mir damit dann glaube ich noch schlechter ...', 6, 0),
(26, 4, 'Sie haben bestimmt nur eine kleine Meinungsverschiedenheit. Sicherlich haben sie das gleich wieder geklärt.', 'Du hattest Recht! Sie haben alles geklärt und haben mir das auch nochmal erklärt, dass es vorkommen kann sich zu streiten.', 4, 5),
(27, 4, 'Ich glaube ich könnte auch nicht aufhören darüber nachzudenken, da musst du jetzt wohl durch.', 'Vermutlich, ja. Aber das macht mich ganz traurig und ich fühle mich Unwohl dabei, so hilflos.', 5, 0),
(30, 4, 'Lass dir bloß nichts anmerken wenn du beim Abendessen bist, sie sollen nicht wissen dass du die Streiterein mitbekommst.', 'Okey, ich versuche meine Emotionen und Tränen zurückzuhalten, aber irgendwie verstärkt das die negativen Gefühle noch zusätzlich...', 6, 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `scenarioplan`
--

CREATE TABLE `scenarioplan` (
  `id` int(32) NOT NULL,
  `userId` int(10) NOT NULL,
  `nextScenarioId` int(10) DEFAULT NULL,
  `nextCategoryId` int(10) DEFAULT NULL,
  `questionOrder` int(8) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `scenarioplan`
--

INSERT INTO `scenarioplan` (`id`, `userId`, `nextScenarioId`, `nextCategoryId`, `questionOrder`) VALUES
(1, 10, 3, 2, 1),
(6, 10, 5, NULL, 3),
(7, 10, NULL, 3, 4),
(11, 12, 4, NULL, 1),
(12, 12, NULL, 3, 2),
(13, 12, 3, NULL, 3),
(14, 12, 5, NULL, 4);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `scenarios`
--

CREATE TABLE `scenarios` (
  `id` int(11) NOT NULL,
  `name` varchar(64) NOT NULL,
  `question` varchar(255) NOT NULL,
  `categoryId` int(11) DEFAULT NULL,
  `creator` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `scenarios`
--

INSERT INTO `scenarios` (`id`, `name`, `question`, `categoryId`, `creator`) VALUES
(3, 'Streit mit Freund', 'Ich habe mich in der Schule mit meinem Freund gestritten. Jetzt bin ich traurig und weis nicht was ich tun soll?', 1, 10),
(4, 'Streit der Eltern', 'Meine Eltern streiten sich unten. Sie schreien sich ganz laut an...', 2, 12),
(5, 'Schlechte Schularbeit', 'Ich habe heute in der Schule bei einer Hausaufgabe viele Fehler gehabt, das beschäftigt mich ein wenig', 3, 16),
(6, 'Fehler beim Fußball', 'Ich habe heute einen Fehler beim Fußball gemacht, und deswegen haben wir verloren...', 4, 16);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `userlevels`
--

CREATE TABLE `userlevels` (
  `level` int(11) NOT NULL,
  `points` int(11) NOT NULL,
  `reward` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `userlevels`
--

INSERT INTO `userlevels` (`level`, `points`, `reward`) VALUES
(1, 10, '1'),
(2, 20, '2'),
(3, 50, '3'),
(4, 100, '4'),
(5, 200, '5');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `users`
--

CREATE TABLE `users` (
  `id` int(10) NOT NULL,
  `username` varchar(16) NOT NULL,
  `password` varchar(255) DEFAULT NULL,
  `lastlogin` datetime NOT NULL,
  `streak` int(10) DEFAULT '0',
  `name` varchar(16) NOT NULL,
  `recordstreak` int(11) DEFAULT '0',
  `email` varchar(32) NOT NULL,
  `registration` datetime NOT NULL,
  `userlevel` int(8) NOT NULL,
  `userpoints` int(16) DEFAULT '0',
  `usertype` int(8) DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `users`
--

INSERT INTO `users` (`id`, `username`, `password`, `lastlogin`, `streak`, `name`, `recordstreak`, `email`, `registration`, `userlevel`, `userpoints`, `usertype`) VALUES
(10, 'testuser', 'betreuteskind', '2025-03-20 13:27:41', 11, 'testuser', 11, '', '2024-12-29 13:41:39', 3, 37, 0),
(12, 'Benjamin', '', '2025-03-07 14:33:58', 2, 'Benji', 0, 'ben@jam.in', '2025-03-08 14:33:58', 1, 2, 0),
(16, 'abcdefgh', '$2y$10$I6WVKiG/HbFg8d/WQwcfFOqhOOT20z949jilDgQXX8D0IbbA.2/12', '2025-03-24 15:05:54', 5, 'abcdefgh', 5, 'asddsa@asd.de', '2025-03-20 15:27:27', 3, 13, 1),
(17, 'Johannes', '$2y$10$AO3pTuolLc644E7.VDMmruh1D86ydZ2D5RIlJp6/PYI3jEqYt7eMe', '2025-03-30 00:19:13', 1, 'Nik', 6, 'kind@te.st', '2025-03-23 16:41:12', 3, 8, 0),
(18, 'ichteste', '$2y$10$qPaqB5t7h.e4nM0ctyNqFeSVMZZcFlXjiVr7yrfyebaYfIeV.KLuG', '2025-03-28 15:12:01', 0, 'ichteste', 0, 'ich@tes.te', '2025-03-28 15:12:01', 1, 0, 0);

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `advisedperson`
--
ALTER TABLE `advisedperson`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_User` (`userid`),
  ADD KEY `FK_Advisor` (`advisorid`);

--
-- Indizes für die Tabelle `answercategories`
--
ALTER TABLE `answercategories`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `diary`
--
ALTER TABLE `diary`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user-diaryentries` (`userid`);

--
-- Indizes für die Tabelle `ercategories`
--
ALTER TABLE `ercategories`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `playedscenarios`
--
ALTER TABLE `playedscenarios`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_PlayedUser` (`userId`),
  ADD KEY `FK_PlayedScenario` (`scenarioId`),
  ADD KEY `FK_AnswerCategory` (`answercategory`),
  ADD KEY `FK_AnswerId` (`answerId`);

--
-- Indizes für die Tabelle `scenarioanswers`
--
ALTER TABLE `scenarioanswers`
  ADD PRIMARY KEY (`id`),
  ADD KEY `Fk_AnswerToScenario` (`scenarioId`),
  ADD KEY `FK_AnswerCategorie` (`answercategory`);

--
-- Indizes für die Tabelle `scenarioplan`
--
ALTER TABLE `scenarioplan`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_ScenarioPlanForUser` (`userId`),
  ADD KEY `FK_NextScenario` (`nextScenarioId`),
  ADD KEY `FK_NextCategory` (`nextCategoryId`) USING BTREE;

--
-- Indizes für die Tabelle `scenarios`
--
ALTER TABLE `scenarios`
  ADD PRIMARY KEY (`id`),
  ADD KEY `Fk_ScenarioCreatedBy` (`creator`),
  ADD KEY `FK_Er` (`categoryId`);

--
-- Indizes für die Tabelle `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `advisedperson`
--
ALTER TABLE `advisedperson`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT für Tabelle `diary`
--
ALTER TABLE `diary`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=49;

--
-- AUTO_INCREMENT für Tabelle `ercategories`
--
ALTER TABLE `ercategories`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT für Tabelle `playedscenarios`
--
ALTER TABLE `playedscenarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=43;

--
-- AUTO_INCREMENT für Tabelle `scenarioanswers`
--
ALTER TABLE `scenarioanswers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT für Tabelle `scenarioplan`
--
ALTER TABLE `scenarioplan`
  MODIFY `id` int(32) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT für Tabelle `scenarios`
--
ALTER TABLE `scenarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT für Tabelle `users`
--
ALTER TABLE `users`
  MODIFY `id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `advisedperson`
--
ALTER TABLE `advisedperson`
  ADD CONSTRAINT `FK_Advisor` FOREIGN KEY (`advisorid`) REFERENCES `users` (`id`),
  ADD CONSTRAINT `FK_User` FOREIGN KEY (`userid`) REFERENCES `users` (`id`);

--
-- Constraints der Tabelle `diary`
--
ALTER TABLE `diary`
  ADD CONSTRAINT `user-diaryentries` FOREIGN KEY (`userid`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints der Tabelle `playedscenarios`
--
ALTER TABLE `playedscenarios`
  ADD CONSTRAINT `FK_AnswerCategory` FOREIGN KEY (`answercategory`) REFERENCES `answercategories` (`id`),
  ADD CONSTRAINT `FK_AnswerId` FOREIGN KEY (`answerId`) REFERENCES `scenarioanswers` (`id`),
  ADD CONSTRAINT `FK_PlayedScenario` FOREIGN KEY (`scenarioId`) REFERENCES `scenarios` (`id`),
  ADD CONSTRAINT `FK_PlayedUser` FOREIGN KEY (`userId`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints der Tabelle `scenarioanswers`
--
ALTER TABLE `scenarioanswers`
  ADD CONSTRAINT `FK_AnswerCategorie` FOREIGN KEY (`answercategory`) REFERENCES `answercategories` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `Fk_AnswerToScenario` FOREIGN KEY (`scenarioId`) REFERENCES `scenarios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints der Tabelle `scenarioplan`
--
ALTER TABLE `scenarioplan`
  ADD CONSTRAINT `FK_ErCategory` FOREIGN KEY (`nextCategoryId`) REFERENCES `ercategories` (`id`),
  ADD CONSTRAINT `FK_NextScenario` FOREIGN KEY (`nextScenarioId`) REFERENCES `scenarios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_ScenarioPlanForUser` FOREIGN KEY (`userId`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints der Tabelle `scenarios`
--
ALTER TABLE `scenarios`
  ADD CONSTRAINT `FK_Er` FOREIGN KEY (`categoryId`) REFERENCES `ercategories` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `Fk_ScenarioCreatedBy` FOREIGN KEY (`creator`) REFERENCES `users` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
