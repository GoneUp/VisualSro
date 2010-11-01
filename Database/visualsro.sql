/*
MySQL Data Transfer
Source Host: localhost
Source Database: visualsro
Target Host: localhost
Target Database: visualsro
Date: 01.11.2010 19:04:11
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for characters
-- ----------------------------
DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters` (
  `id` int(10) unsigned NOT NULL,
  `account` int(10) unsigned NOT NULL,
  `name` varchar(45) NOT NULL,
  `chartype` int(10) unsigned NOT NULL DEFAULT '1907',
  `volume` int(10) unsigned NOT NULL DEFAULT '34',
  `level` int(10) unsigned NOT NULL DEFAULT '1',
  `experience` int(10) unsigned NOT NULL DEFAULT '0',
  `strength` int(10) unsigned NOT NULL DEFAULT '20',
  `intelligence` int(10) unsigned NOT NULL DEFAULT '30',
  `attribute` int(10) unsigned NOT NULL DEFAULT '0',
  `hp` int(10) unsigned NOT NULL DEFAULT '200',
  `mp` int(10) unsigned NOT NULL DEFAULT '300',
  `deletion_mark` tinyint(1) NOT NULL DEFAULT '0',
  `deletion_time` datetime NOT NULL DEFAULT '1000-01-01 00:00:00' COMMENT 'Datum',
  `gold` bigint(50) unsigned NOT NULL DEFAULT '0',
  `sp` bigint(10) unsigned NOT NULL DEFAULT '0',
  `gm` tinyint(1) NOT NULL DEFAULT '0',
  `xsect` int(10) unsigned NOT NULL DEFAULT '168' COMMENT 'Default loc Jangan',
  `ysect` int(10) unsigned NOT NULL DEFAULT '98',
  `xpos` int(10) unsigned NOT NULL DEFAULT '978',
  `ypos` int(10) unsigned NOT NULL DEFAULT '1097',
  `zpos` bigint(10) NOT NULL DEFAULT '40',
  `cur_hp` int(10) unsigned NOT NULL DEFAULT '200',
  `cur_mp` int(10) unsigned NOT NULL DEFAULT '300',
  `min_phyatk` int(10) unsigned NOT NULL DEFAULT '5',
  `max_phyatk` int(10) unsigned NOT NULL DEFAULT '10',
  `min_magatk` int(10) unsigned NOT NULL DEFAULT '5',
  `max_magatk` int(10) unsigned NOT NULL DEFAULT '10',
  `phydef` int(10) unsigned NOT NULL DEFAULT '9',
  `magdef` int(10) unsigned NOT NULL DEFAULT '7',
  `hit` int(10) unsigned NOT NULL DEFAULT '3',
  `parry` int(10) unsigned NOT NULL DEFAULT '5',
  `walkspeed` int(10) unsigned NOT NULL DEFAULT '16',
  `runspeed` int(10) unsigned NOT NULL DEFAULT '50',
  `berserkspeed` int(10) unsigned NOT NULL DEFAULT '100',
  `berserking` tinyint(1) NOT NULL DEFAULT '0',
  `pvp` int(10) unsigned NOT NULL DEFAULT '255',
  `maxitemslots` int(1) NOT NULL DEFAULT '45' COMMENT 'xycvyyxvcxy',
  `helpericon` int(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for hotkeys
-- ----------------------------
DROP TABLE IF EXISTS `hotkeys`;
CREATE TABLE `hotkeys` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `OwnerID` int(10) NOT NULL,
  `Slot` int(10) NOT NULL,
  `Type` int(10) NOT NULL DEFAULT '0',
  `IconID` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=257 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for items
-- ----------------------------
DROP TABLE IF EXISTS `items`;
CREATE TABLE `items` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `itemtype` int(10) unsigned NOT NULL,
  `owner` int(10) NOT NULL,
  `plusvalue` int(10) unsigned NOT NULL DEFAULT '0',
  `slot` int(10) unsigned NOT NULL,
  `quantity` int(10) unsigned NOT NULL DEFAULT '1',
  `durability` int(10) unsigned NOT NULL DEFAULT '30',
  `itemnumber` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1204 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for log
-- ----------------------------
DROP TABLE IF EXISTS `log`;
CREATE TABLE `log` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `ip_adress` varchar(255) NOT NULL DEFAULT '0.0.0.0',
  `charname` varchar(255) NOT NULL,
  `action` varchar(255) NOT NULL COMMENT 'Types: Login, GM, Chat, Register',
  `action2` varchar(255) NOT NULL COMMENT 'SubTypes: Login (None); GM (Item Create, Command, Ban); Chat (Normal, GM, Party, Guild, Whisper, Notice)',
  `parameter` longtext NOT NULL COMMENT 'Ex. Char Message',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for masteries
-- ----------------------------
DROP TABLE IF EXISTS `masteries`;
CREATE TABLE `masteries` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `owner` int(10) unsigned NOT NULL,
  `mastery` int(10) unsigned NOT NULL,
  `level` int(10) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=276 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for news
-- ----------------------------
DROP TABLE IF EXISTS `news`;
CREATE TABLE `news` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `head` varchar(30) DEFAULT NULL,
  `text` varchar(250) DEFAULT NULL,
  `day` int(2) DEFAULT NULL,
  `month` int(2) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for positions
-- ----------------------------
DROP TABLE IF EXISTS `positions`;
CREATE TABLE `positions` (
  `OwnerCharID` int(10) NOT NULL,
  `return_xsect` int(10) unsigned NOT NULL DEFAULT '168' COMMENT 'jangan -- Point for Return Scroll, Respawn when dead',
  `return_ysect` int(10) unsigned NOT NULL DEFAULT '97',
  `return_xpos` int(10) unsigned NOT NULL DEFAULT '980',
  `return_ypos` int(10) unsigned NOT NULL DEFAULT '1330',
  `return_zpos` int(10) NOT NULL DEFAULT '65504',
  `recall_xsect` int(10) unsigned NOT NULL DEFAULT '168' COMMENT 'jangan -- Point of Last Return Scroll usage',
  `recall_ysect` int(10) unsigned NOT NULL DEFAULT '97',
  `recall_xpos` int(10) unsigned NOT NULL DEFAULT '980',
  `recall_ypos` int(10) unsigned NOT NULL DEFAULT '1330',
  `recall_zpos` int(10) NOT NULL DEFAULT '65504',
  `dead_xsect` int(10) unsigned NOT NULL DEFAULT '168' COMMENT 'jangan -- Point of Last Dead',
  `dead_ysect` int(10) unsigned NOT NULL DEFAULT '97',
  `deadl_xpos` int(10) unsigned NOT NULL DEFAULT '980',
  `dead_ypos` int(10) unsigned NOT NULL DEFAULT '1330',
  `dead_zpos` int(10) NOT NULL DEFAULT '65504',
  PRIMARY KEY (`OwnerCharID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for quests
-- ----------------------------
DROP TABLE IF EXISTS `quests`;
CREATE TABLE `quests` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `owner` int(10) unsigned NOT NULL,
  `quest` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for servers
-- ----------------------------
DROP TABLE IF EXISTS `servers`;
CREATE TABLE `servers` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `users_current` int(10) unsigned NOT NULL DEFAULT '0',
  `users_max` int(10) unsigned NOT NULL DEFAULT '500',
  `state` int(10) unsigned NOT NULL DEFAULT '1',
  `ip` varchar(45) NOT NULL,
  `port` int(10) unsigned NOT NULL DEFAULT '15000',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for skills
-- ----------------------------
DROP TABLE IF EXISTS `skills`;
CREATE TABLE `skills` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `Owner` int(10) NOT NULL,
  `SkillID` int(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `failed_logins` int(10) unsigned NOT NULL DEFAULT '0',
  `banned` int(1) NOT NULL DEFAULT '0',
  `banreason` varchar(255) NOT NULL DEFAULT '...',
  `bantime` datetime NOT NULL DEFAULT '3000-01-01 00:00:00',
  `silk` int(10) unsigned NOT NULL DEFAULT '500',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `characters` VALUES ('1', '3', 'Over', '1907', '34', '100', '0', '20', '20', '0', '1421', '1421', '0', '1000-01-01 00:00:00', '1000000', '1000000', '-1', '168', '97', '1556', '1610', '0', '200', '300', '116', '214', '163', '163', '9', '6', '110', '110', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `hotkeys` VALUES ('206', '1', '0', '0', '0');
INSERT INTO `hotkeys` VALUES ('207', '1', '1', '0', '0');
INSERT INTO `hotkeys` VALUES ('208', '1', '2', '0', '0');
INSERT INTO `hotkeys` VALUES ('209', '1', '3', '0', '0');
INSERT INTO `hotkeys` VALUES ('210', '1', '4', '0', '0');
INSERT INTO `hotkeys` VALUES ('211', '1', '5', '0', '0');
INSERT INTO `hotkeys` VALUES ('212', '1', '6', '0', '0');
INSERT INTO `hotkeys` VALUES ('213', '1', '7', '0', '0');
INSERT INTO `hotkeys` VALUES ('214', '1', '8', '0', '0');
INSERT INTO `hotkeys` VALUES ('215', '1', '9', '0', '0');
INSERT INTO `hotkeys` VALUES ('216', '1', '10', '0', '0');
INSERT INTO `hotkeys` VALUES ('217', '1', '11', '0', '0');
INSERT INTO `hotkeys` VALUES ('218', '1', '12', '0', '0');
INSERT INTO `hotkeys` VALUES ('219', '1', '13', '0', '0');
INSERT INTO `hotkeys` VALUES ('220', '1', '14', '0', '0');
INSERT INTO `hotkeys` VALUES ('221', '1', '15', '0', '0');
INSERT INTO `hotkeys` VALUES ('222', '1', '16', '0', '0');
INSERT INTO `hotkeys` VALUES ('223', '1', '17', '0', '0');
INSERT INTO `hotkeys` VALUES ('224', '1', '18', '0', '0');
INSERT INTO `hotkeys` VALUES ('225', '1', '19', '0', '0');
INSERT INTO `hotkeys` VALUES ('226', '1', '20', '0', '0');
INSERT INTO `hotkeys` VALUES ('227', '1', '21', '0', '0');
INSERT INTO `hotkeys` VALUES ('228', '1', '22', '0', '0');
INSERT INTO `hotkeys` VALUES ('229', '1', '23', '0', '0');
INSERT INTO `hotkeys` VALUES ('230', '1', '24', '0', '0');
INSERT INTO `hotkeys` VALUES ('231', '1', '25', '0', '0');
INSERT INTO `hotkeys` VALUES ('232', '1', '26', '0', '0');
INSERT INTO `hotkeys` VALUES ('233', '1', '27', '0', '0');
INSERT INTO `hotkeys` VALUES ('234', '1', '28', '0', '0');
INSERT INTO `hotkeys` VALUES ('235', '1', '29', '0', '0');
INSERT INTO `hotkeys` VALUES ('236', '1', '30', '0', '0');
INSERT INTO `hotkeys` VALUES ('237', '1', '31', '0', '0');
INSERT INTO `hotkeys` VALUES ('238', '1', '32', '0', '0');
INSERT INTO `hotkeys` VALUES ('239', '1', '33', '0', '0');
INSERT INTO `hotkeys` VALUES ('240', '1', '34', '0', '0');
INSERT INTO `hotkeys` VALUES ('241', '1', '35', '0', '0');
INSERT INTO `hotkeys` VALUES ('242', '1', '36', '0', '0');
INSERT INTO `hotkeys` VALUES ('243', '1', '37', '0', '0');
INSERT INTO `hotkeys` VALUES ('244', '1', '38', '0', '0');
INSERT INTO `hotkeys` VALUES ('245', '1', '39', '0', '0');
INSERT INTO `hotkeys` VALUES ('246', '1', '40', '0', '0');
INSERT INTO `hotkeys` VALUES ('247', '1', '41', '0', '0');
INSERT INTO `hotkeys` VALUES ('248', '1', '42', '0', '0');
INSERT INTO `hotkeys` VALUES ('249', '1', '43', '0', '0');
INSERT INTO `hotkeys` VALUES ('250', '1', '44', '0', '0');
INSERT INTO `hotkeys` VALUES ('251', '1', '45', '0', '0');
INSERT INTO `hotkeys` VALUES ('252', '1', '46', '0', '0');
INSERT INTO `hotkeys` VALUES ('253', '1', '47', '0', '0');
INSERT INTO `hotkeys` VALUES ('254', '1', '48', '0', '0');
INSERT INTO `hotkeys` VALUES ('255', '1', '49', '0', '0');
INSERT INTO `hotkeys` VALUES ('256', '1', '50', '0', '0');
INSERT INTO `items` VALUES ('1159', '0', '1', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('1160', '3640', '1', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('1161', '0', '1', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('1162', '0', '1', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('1163', '3641', '1', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('1164', '3642', '1', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('1165', '3635', '1', '1', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('1166', '0', '1', '0', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('1167', '0', '1', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('1168', '0', '1', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('1169', '0', '1', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('1170', '0', '1', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('1171', '0', '1', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('1172', '3851', '1', '0', '13', '11', '30', 'item13');
INSERT INTO `items` VALUES ('1173', '8985', '1', '0', '14', '1', '30', 'item14');
INSERT INTO `items` VALUES ('1174', '3829', '1', '0', '15', '1', '30', 'item15');
INSERT INTO `items` VALUES ('1175', '3795', '1', '0', '16', '11', '30', 'item16');
INSERT INTO `items` VALUES ('1176', '3769', '1', '0', '17', '11', '30', 'item17');
INSERT INTO `items` VALUES ('1177', '5912', '1', '0', '18', '1000', '30', 'item18');
INSERT INTO `items` VALUES ('1178', '5913', '1', '0', '19', '1000', '30', 'item19');
INSERT INTO `items` VALUES ('1179', '3823', '1', '0', '20', '10000', '30', 'item20');
INSERT INTO `items` VALUES ('1180', '24405', '1', '0', '21', '1', '30', 'item21');
INSERT INTO `items` VALUES ('1181', '24406', '1', '0', '22', '1', '30', 'item22');
INSERT INTO `items` VALUES ('1182', '23297', '1', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('1183', '23299', '1', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('1184', '23293', '1', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('1185', '24311', '1', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('1186', '24286', '1', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('1187', '0', '1', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('1188', '0', '1', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('1189', '0', '1', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('1190', '0', '1', '0', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('1191', '0', '1', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('1192', '0', '1', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('1193', '0', '1', '0', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('1194', '0', '1', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('1195', '0', '1', '0', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('1196', '0', '1', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('1197', '0', '1', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('1198', '0', '1', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('1199', '0', '1', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('1200', '0', '1', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('1201', '0', '1', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('1202', '0', '1', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('1203', '0', '1', '0', '44', '0', '30', 'item44');
INSERT INTO `log` VALUES ('1', '0000-00-00 00:00:00', '127.0.0.1:59141', '[UNKNOWN]', 'Login', 'Sucess', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('2', '0000-00-00 00:00:00', '127.0.0.1:59143', '[UNKNOWN]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('3', '0000-00-00 00:00:00', '127.0.0.1:59143', '[UNKNOWN]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('4', '0000-00-00 00:00:00', '127.0.0.1:59150', '[UNKNOWN]', 'Login', 'Sucess', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('5', '0000-00-00 00:00:00', '127.0.0.1:59696', '[UNKNOWN]', 'Login', 'Sucess', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('6', '0000-00-00 00:00:00', '127.0.0.1:59805', '[UNKNOWN]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('7', '2010-11-01 19:00:54', '127.0.0.1:59832', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('8', '2010-11-01 19:02:30', '127.0.0.1:59832', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `masteries` VALUES ('269', '1', '257', '0');
INSERT INTO `masteries` VALUES ('270', '1', '258', '0');
INSERT INTO `masteries` VALUES ('271', '1', '259', '0');
INSERT INTO `masteries` VALUES ('272', '1', '273', '0');
INSERT INTO `masteries` VALUES ('273', '1', '274', '0');
INSERT INTO `masteries` VALUES ('274', '1', '275', '0');
INSERT INTO `masteries` VALUES ('275', '1', '276', '0');
INSERT INTO `news` VALUES ('1', 'Opening', '<center><font color=red><b>Welcome to the Visual Silkroad Project</b></font></center>', '25', '4');
INSERT INTO `positions` VALUES ('1', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `servers` VALUES ('3', 'Local', '0', '500', '1', '127.0.0.1', '15780');
INSERT INTO `servers` VALUES ('4', 'Remote', '0', '500', '1', '78.111.78.27', '15780');
INSERT INTO `users` VALUES ('3', 'i', 'i', '0', '0', 'You got banned for 10 Minutes because of 5 failed Logins.', '2010-10-01 16:01:38', '6578');
INSERT INTO `users` VALUES ('9', 'test', 'test', '0', '0', '...', '3000-01-01 00:00:00', '500');
