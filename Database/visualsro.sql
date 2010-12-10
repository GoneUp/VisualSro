/*
MySQL Data Transfer
Source Host: localhost
Source Database: visualsro
Target Host: localhost
Target Database: visualsro
Date: 11.12.2010 00:03:38
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
  `xpos` bigint(10) NOT NULL DEFAULT '978',
  `ypos` bigint(10) NOT NULL DEFAULT '1097',
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
-- Table structure for guild_main
-- ----------------------------
DROP TABLE IF EXISTS `guild_main`;
CREATE TABLE `guild_main` (
  `guildid` int(10) unsigned NOT NULL,
  `name` varchar(255) NOT NULL,
  `gp` int(10) unsigned NOT NULL DEFAULT '0',
  `level` int(10) NOT NULL DEFAULT '1',
  `notice_title` varchar(255) NOT NULL DEFAULT '--',
  `notice` varchar(255) NOT NULL DEFAULT '--',
  PRIMARY KEY (`guildid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for guild_member
-- ----------------------------
DROP TABLE IF EXISTS `guild_member`;
CREATE TABLE `guild_member` (
  `charid` int(255) NOT NULL AUTO_INCREMENT,
  `guildid` bigint(255) NOT NULL DEFAULT '-1',
  `guildpoints` int(255) NOT NULL DEFAULT '0',
  `grantname` varchar(255) NOT NULL DEFAULT '',
  `master` int(255) NOT NULL DEFAULT '0',
  `invite` int(255) NOT NULL DEFAULT '0',
  `kick` int(255) NOT NULL DEFAULT '0',
  `notice` int(255) NOT NULL DEFAULT '0',
  `union` int(255) NOT NULL DEFAULT '0',
  `storage` int(255) NOT NULL DEFAULT '0',
  PRIMARY KEY (`charid`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

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
) ENGINE=InnoDB AUTO_INCREMENT=563 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=1474 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=447 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=317 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for news
-- ----------------------------
DROP TABLE IF EXISTS `news`;
CREATE TABLE `news` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `head` varchar(30) NOT NULL,
  `text` varchar(255) NOT NULL,
  `datetime` datetime NOT NULL DEFAULT '2010-01-01 00:00:00',
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
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `characters` VALUES ('1', '3', 'Over', '1919', '34', '140', '994267238', '852', '203', '0', '133618', '31836', '0', '1000-01-01 00:00:00', '1000000', '3485753', '-1', '79', '105', '242', '1600', '79', '6760', '1421', '283', '426', '68', '68', '336', '67', '150', '150', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `characters` VALUES ('8780', '9', 'Build', '1907', '34', '100', '0', '20', '20', '0', '1421', '1421', '0', '1000-01-01 00:00:00', '1000000', '1000000', '-1', '168', '98', '964', '24', '4', '200', '300', '936', '1121', '1579', '1579', '59', '6', '110', '110', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `characters` VALUES ('8781', '9', '___', '14731', '65', '100', '0', '20', '20', '0', '1421', '1421', '0', '1000-01-01 00:00:00', '1000000', '1000000', '-1', '153', '102', '944', '412', '-105', '200', '300', '6', '109', '1954', '1954', '53', '6', '110', '110', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `characters` VALUES ('8782', '10', 'se', '1907', '34', '100', '0', '20', '20', '0', '1421', '1421', '0', '1000-01-01 00:00:00', '1000000', '1000000', '-1', '168', '96', '970', '1623', '0', '200', '300', '850', '1021', '1439', '1439', '86', '206', '110', '110', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `characters` VALUES ('8783', '12', '_1_2_3_4_5_', '1911', '68', '100', '0', '20', '20', '0', '1421', '1421', '0', '1000-01-01 00:00:00', '1000000', '1000000', '-1', '153', '102', '907', '23', '-104', '200', '300', '1026', '1227', '1457', '1457', '59', '6', '110', '110', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `characters` VALUES ('8785', '9', 'manneke', '1907', '34', '100', '0', '20', '20', '0', '1421', '1421', '0', '1000-01-01 00:00:00', '1000000', '1000000', '-1', '168', '96', '972', '1420', '-5', '200', '300', '850', '1021', '1439', '1439', '86', '206', '110', '110', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `characters` VALUES ('8840', '13', 'dongdong', '1919', '34', '90', '0', '20', '20', '0', '1165', '1165', '0', '1000-01-01 00:00:00', '1000000', '1000000', '-1', '168', '97', '966', '1649', '0', '200', '300', '99', '200', '151', '151', '6', '6', '100', '100', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `guild_main` VALUES ('1', 'First', '0', '1', '--', '--');
INSERT INTO `guild_member` VALUES ('1', '1', '0', '', '1', '0', '0', '0', '0', '0');
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
INSERT INTO `hotkeys` VALUES ('257', '8780', '0', '0', '0');
INSERT INTO `hotkeys` VALUES ('258', '8780', '1', '0', '0');
INSERT INTO `hotkeys` VALUES ('259', '8780', '2', '0', '0');
INSERT INTO `hotkeys` VALUES ('260', '8780', '3', '0', '0');
INSERT INTO `hotkeys` VALUES ('261', '8780', '4', '0', '0');
INSERT INTO `hotkeys` VALUES ('262', '8780', '5', '0', '0');
INSERT INTO `hotkeys` VALUES ('263', '8780', '6', '0', '0');
INSERT INTO `hotkeys` VALUES ('264', '8780', '7', '0', '0');
INSERT INTO `hotkeys` VALUES ('265', '8780', '8', '0', '0');
INSERT INTO `hotkeys` VALUES ('266', '8780', '9', '0', '0');
INSERT INTO `hotkeys` VALUES ('267', '8780', '10', '0', '0');
INSERT INTO `hotkeys` VALUES ('268', '8780', '11', '0', '0');
INSERT INTO `hotkeys` VALUES ('269', '8780', '12', '0', '0');
INSERT INTO `hotkeys` VALUES ('270', '8780', '13', '0', '0');
INSERT INTO `hotkeys` VALUES ('271', '8780', '14', '0', '0');
INSERT INTO `hotkeys` VALUES ('272', '8780', '15', '0', '0');
INSERT INTO `hotkeys` VALUES ('273', '8780', '16', '0', '0');
INSERT INTO `hotkeys` VALUES ('274', '8780', '17', '0', '0');
INSERT INTO `hotkeys` VALUES ('275', '8780', '18', '0', '0');
INSERT INTO `hotkeys` VALUES ('276', '8780', '19', '0', '0');
INSERT INTO `hotkeys` VALUES ('277', '8780', '20', '0', '0');
INSERT INTO `hotkeys` VALUES ('278', '8780', '21', '0', '0');
INSERT INTO `hotkeys` VALUES ('279', '8780', '22', '0', '0');
INSERT INTO `hotkeys` VALUES ('280', '8780', '23', '0', '0');
INSERT INTO `hotkeys` VALUES ('281', '8780', '24', '0', '0');
INSERT INTO `hotkeys` VALUES ('282', '8780', '25', '0', '0');
INSERT INTO `hotkeys` VALUES ('283', '8780', '26', '0', '0');
INSERT INTO `hotkeys` VALUES ('284', '8780', '27', '0', '0');
INSERT INTO `hotkeys` VALUES ('285', '8780', '28', '0', '0');
INSERT INTO `hotkeys` VALUES ('286', '8780', '29', '0', '0');
INSERT INTO `hotkeys` VALUES ('287', '8780', '30', '0', '0');
INSERT INTO `hotkeys` VALUES ('288', '8780', '31', '0', '0');
INSERT INTO `hotkeys` VALUES ('289', '8780', '32', '0', '0');
INSERT INTO `hotkeys` VALUES ('290', '8780', '33', '0', '0');
INSERT INTO `hotkeys` VALUES ('291', '8780', '34', '0', '0');
INSERT INTO `hotkeys` VALUES ('292', '8780', '35', '0', '0');
INSERT INTO `hotkeys` VALUES ('293', '8780', '36', '0', '0');
INSERT INTO `hotkeys` VALUES ('294', '8780', '37', '0', '0');
INSERT INTO `hotkeys` VALUES ('295', '8780', '38', '0', '0');
INSERT INTO `hotkeys` VALUES ('296', '8780', '39', '0', '0');
INSERT INTO `hotkeys` VALUES ('297', '8780', '40', '0', '0');
INSERT INTO `hotkeys` VALUES ('298', '8780', '41', '0', '0');
INSERT INTO `hotkeys` VALUES ('299', '8780', '42', '0', '0');
INSERT INTO `hotkeys` VALUES ('300', '8780', '43', '0', '0');
INSERT INTO `hotkeys` VALUES ('301', '8780', '44', '0', '0');
INSERT INTO `hotkeys` VALUES ('302', '8780', '45', '0', '0');
INSERT INTO `hotkeys` VALUES ('303', '8780', '46', '0', '0');
INSERT INTO `hotkeys` VALUES ('304', '8780', '47', '0', '0');
INSERT INTO `hotkeys` VALUES ('305', '8780', '48', '0', '0');
INSERT INTO `hotkeys` VALUES ('306', '8780', '49', '0', '0');
INSERT INTO `hotkeys` VALUES ('307', '8780', '50', '0', '0');
INSERT INTO `hotkeys` VALUES ('308', '8781', '0', '0', '0');
INSERT INTO `hotkeys` VALUES ('309', '8781', '1', '0', '0');
INSERT INTO `hotkeys` VALUES ('310', '8781', '2', '0', '0');
INSERT INTO `hotkeys` VALUES ('311', '8781', '3', '0', '0');
INSERT INTO `hotkeys` VALUES ('312', '8781', '4', '0', '0');
INSERT INTO `hotkeys` VALUES ('313', '8781', '5', '0', '0');
INSERT INTO `hotkeys` VALUES ('314', '8781', '6', '0', '0');
INSERT INTO `hotkeys` VALUES ('315', '8781', '7', '0', '0');
INSERT INTO `hotkeys` VALUES ('316', '8781', '8', '0', '0');
INSERT INTO `hotkeys` VALUES ('317', '8781', '9', '0', '0');
INSERT INTO `hotkeys` VALUES ('318', '8781', '10', '0', '0');
INSERT INTO `hotkeys` VALUES ('319', '8781', '11', '0', '0');
INSERT INTO `hotkeys` VALUES ('320', '8781', '12', '0', '0');
INSERT INTO `hotkeys` VALUES ('321', '8781', '13', '0', '0');
INSERT INTO `hotkeys` VALUES ('322', '8781', '14', '0', '0');
INSERT INTO `hotkeys` VALUES ('323', '8781', '15', '0', '0');
INSERT INTO `hotkeys` VALUES ('324', '8781', '16', '0', '0');
INSERT INTO `hotkeys` VALUES ('325', '8781', '17', '0', '0');
INSERT INTO `hotkeys` VALUES ('326', '8781', '18', '0', '0');
INSERT INTO `hotkeys` VALUES ('327', '8781', '19', '0', '0');
INSERT INTO `hotkeys` VALUES ('328', '8781', '20', '0', '0');
INSERT INTO `hotkeys` VALUES ('329', '8781', '21', '0', '0');
INSERT INTO `hotkeys` VALUES ('330', '8781', '22', '0', '0');
INSERT INTO `hotkeys` VALUES ('331', '8781', '23', '0', '0');
INSERT INTO `hotkeys` VALUES ('332', '8781', '24', '0', '0');
INSERT INTO `hotkeys` VALUES ('333', '8781', '25', '0', '0');
INSERT INTO `hotkeys` VALUES ('334', '8781', '26', '0', '0');
INSERT INTO `hotkeys` VALUES ('335', '8781', '27', '0', '0');
INSERT INTO `hotkeys` VALUES ('336', '8781', '28', '0', '0');
INSERT INTO `hotkeys` VALUES ('337', '8781', '29', '0', '0');
INSERT INTO `hotkeys` VALUES ('338', '8781', '30', '0', '0');
INSERT INTO `hotkeys` VALUES ('339', '8781', '31', '0', '0');
INSERT INTO `hotkeys` VALUES ('340', '8781', '32', '0', '0');
INSERT INTO `hotkeys` VALUES ('341', '8781', '33', '0', '0');
INSERT INTO `hotkeys` VALUES ('342', '8781', '34', '0', '0');
INSERT INTO `hotkeys` VALUES ('343', '8781', '35', '0', '0');
INSERT INTO `hotkeys` VALUES ('344', '8781', '36', '0', '0');
INSERT INTO `hotkeys` VALUES ('345', '8781', '37', '0', '0');
INSERT INTO `hotkeys` VALUES ('346', '8781', '38', '0', '0');
INSERT INTO `hotkeys` VALUES ('347', '8781', '39', '0', '0');
INSERT INTO `hotkeys` VALUES ('348', '8781', '40', '0', '0');
INSERT INTO `hotkeys` VALUES ('349', '8781', '41', '0', '0');
INSERT INTO `hotkeys` VALUES ('350', '8781', '42', '0', '0');
INSERT INTO `hotkeys` VALUES ('351', '8781', '43', '0', '0');
INSERT INTO `hotkeys` VALUES ('352', '8781', '44', '0', '0');
INSERT INTO `hotkeys` VALUES ('353', '8781', '45', '0', '0');
INSERT INTO `hotkeys` VALUES ('354', '8781', '46', '0', '0');
INSERT INTO `hotkeys` VALUES ('355', '8781', '47', '0', '0');
INSERT INTO `hotkeys` VALUES ('356', '8781', '48', '0', '0');
INSERT INTO `hotkeys` VALUES ('357', '8781', '49', '0', '0');
INSERT INTO `hotkeys` VALUES ('358', '8781', '50', '0', '0');
INSERT INTO `hotkeys` VALUES ('359', '8782', '0', '0', '0');
INSERT INTO `hotkeys` VALUES ('360', '8782', '1', '0', '0');
INSERT INTO `hotkeys` VALUES ('361', '8782', '2', '0', '0');
INSERT INTO `hotkeys` VALUES ('362', '8782', '3', '0', '0');
INSERT INTO `hotkeys` VALUES ('363', '8782', '4', '0', '0');
INSERT INTO `hotkeys` VALUES ('364', '8782', '5', '0', '0');
INSERT INTO `hotkeys` VALUES ('365', '8782', '6', '0', '0');
INSERT INTO `hotkeys` VALUES ('366', '8782', '7', '0', '0');
INSERT INTO `hotkeys` VALUES ('367', '8782', '8', '0', '0');
INSERT INTO `hotkeys` VALUES ('368', '8782', '9', '0', '0');
INSERT INTO `hotkeys` VALUES ('369', '8782', '10', '0', '0');
INSERT INTO `hotkeys` VALUES ('370', '8782', '11', '0', '0');
INSERT INTO `hotkeys` VALUES ('371', '8782', '12', '0', '0');
INSERT INTO `hotkeys` VALUES ('372', '8782', '13', '0', '0');
INSERT INTO `hotkeys` VALUES ('373', '8782', '14', '0', '0');
INSERT INTO `hotkeys` VALUES ('374', '8782', '15', '0', '0');
INSERT INTO `hotkeys` VALUES ('375', '8782', '16', '0', '0');
INSERT INTO `hotkeys` VALUES ('376', '8782', '17', '0', '0');
INSERT INTO `hotkeys` VALUES ('377', '8782', '18', '0', '0');
INSERT INTO `hotkeys` VALUES ('378', '8782', '19', '0', '0');
INSERT INTO `hotkeys` VALUES ('379', '8782', '20', '0', '0');
INSERT INTO `hotkeys` VALUES ('380', '8782', '21', '0', '0');
INSERT INTO `hotkeys` VALUES ('381', '8782', '22', '0', '0');
INSERT INTO `hotkeys` VALUES ('382', '8782', '23', '0', '0');
INSERT INTO `hotkeys` VALUES ('383', '8782', '24', '0', '0');
INSERT INTO `hotkeys` VALUES ('384', '8782', '25', '0', '0');
INSERT INTO `hotkeys` VALUES ('385', '8782', '26', '0', '0');
INSERT INTO `hotkeys` VALUES ('386', '8782', '27', '0', '0');
INSERT INTO `hotkeys` VALUES ('387', '8782', '28', '0', '0');
INSERT INTO `hotkeys` VALUES ('388', '8782', '29', '0', '0');
INSERT INTO `hotkeys` VALUES ('389', '8782', '30', '0', '0');
INSERT INTO `hotkeys` VALUES ('390', '8782', '31', '0', '0');
INSERT INTO `hotkeys` VALUES ('391', '8782', '32', '0', '0');
INSERT INTO `hotkeys` VALUES ('392', '8782', '33', '0', '0');
INSERT INTO `hotkeys` VALUES ('393', '8782', '34', '0', '0');
INSERT INTO `hotkeys` VALUES ('394', '8782', '35', '0', '0');
INSERT INTO `hotkeys` VALUES ('395', '8782', '36', '0', '0');
INSERT INTO `hotkeys` VALUES ('396', '8782', '37', '0', '0');
INSERT INTO `hotkeys` VALUES ('397', '8782', '38', '0', '0');
INSERT INTO `hotkeys` VALUES ('398', '8782', '39', '0', '0');
INSERT INTO `hotkeys` VALUES ('399', '8782', '40', '0', '0');
INSERT INTO `hotkeys` VALUES ('400', '8782', '41', '0', '0');
INSERT INTO `hotkeys` VALUES ('401', '8782', '42', '0', '0');
INSERT INTO `hotkeys` VALUES ('402', '8782', '43', '0', '0');
INSERT INTO `hotkeys` VALUES ('403', '8782', '44', '0', '0');
INSERT INTO `hotkeys` VALUES ('404', '8782', '45', '0', '0');
INSERT INTO `hotkeys` VALUES ('405', '8782', '46', '0', '0');
INSERT INTO `hotkeys` VALUES ('406', '8782', '47', '0', '0');
INSERT INTO `hotkeys` VALUES ('407', '8782', '48', '0', '0');
INSERT INTO `hotkeys` VALUES ('408', '8782', '49', '0', '0');
INSERT INTO `hotkeys` VALUES ('409', '8782', '50', '0', '0');
INSERT INTO `hotkeys` VALUES ('410', '8783', '0', '0', '0');
INSERT INTO `hotkeys` VALUES ('411', '8783', '1', '0', '0');
INSERT INTO `hotkeys` VALUES ('412', '8783', '2', '0', '0');
INSERT INTO `hotkeys` VALUES ('413', '8783', '3', '0', '0');
INSERT INTO `hotkeys` VALUES ('414', '8783', '4', '0', '0');
INSERT INTO `hotkeys` VALUES ('415', '8783', '5', '0', '0');
INSERT INTO `hotkeys` VALUES ('416', '8783', '6', '0', '0');
INSERT INTO `hotkeys` VALUES ('417', '8783', '7', '0', '0');
INSERT INTO `hotkeys` VALUES ('418', '8783', '8', '0', '0');
INSERT INTO `hotkeys` VALUES ('419', '8783', '9', '0', '0');
INSERT INTO `hotkeys` VALUES ('420', '8783', '10', '0', '0');
INSERT INTO `hotkeys` VALUES ('421', '8783', '11', '0', '0');
INSERT INTO `hotkeys` VALUES ('422', '8783', '12', '0', '0');
INSERT INTO `hotkeys` VALUES ('423', '8783', '13', '0', '0');
INSERT INTO `hotkeys` VALUES ('424', '8783', '14', '0', '0');
INSERT INTO `hotkeys` VALUES ('425', '8783', '15', '0', '0');
INSERT INTO `hotkeys` VALUES ('426', '8783', '16', '0', '0');
INSERT INTO `hotkeys` VALUES ('427', '8783', '17', '0', '0');
INSERT INTO `hotkeys` VALUES ('428', '8783', '18', '0', '0');
INSERT INTO `hotkeys` VALUES ('429', '8783', '19', '0', '0');
INSERT INTO `hotkeys` VALUES ('430', '8783', '20', '0', '0');
INSERT INTO `hotkeys` VALUES ('431', '8783', '21', '0', '0');
INSERT INTO `hotkeys` VALUES ('432', '8783', '22', '0', '0');
INSERT INTO `hotkeys` VALUES ('433', '8783', '23', '0', '0');
INSERT INTO `hotkeys` VALUES ('434', '8783', '24', '0', '0');
INSERT INTO `hotkeys` VALUES ('435', '8783', '25', '0', '0');
INSERT INTO `hotkeys` VALUES ('436', '8783', '26', '0', '0');
INSERT INTO `hotkeys` VALUES ('437', '8783', '27', '0', '0');
INSERT INTO `hotkeys` VALUES ('438', '8783', '28', '0', '0');
INSERT INTO `hotkeys` VALUES ('439', '8783', '29', '0', '0');
INSERT INTO `hotkeys` VALUES ('440', '8783', '30', '0', '0');
INSERT INTO `hotkeys` VALUES ('441', '8783', '31', '0', '0');
INSERT INTO `hotkeys` VALUES ('442', '8783', '32', '0', '0');
INSERT INTO `hotkeys` VALUES ('443', '8783', '33', '0', '0');
INSERT INTO `hotkeys` VALUES ('444', '8783', '34', '0', '0');
INSERT INTO `hotkeys` VALUES ('445', '8783', '35', '0', '0');
INSERT INTO `hotkeys` VALUES ('446', '8783', '36', '0', '0');
INSERT INTO `hotkeys` VALUES ('447', '8783', '37', '0', '0');
INSERT INTO `hotkeys` VALUES ('448', '8783', '38', '0', '0');
INSERT INTO `hotkeys` VALUES ('449', '8783', '39', '0', '0');
INSERT INTO `hotkeys` VALUES ('450', '8783', '40', '0', '0');
INSERT INTO `hotkeys` VALUES ('451', '8783', '41', '0', '0');
INSERT INTO `hotkeys` VALUES ('452', '8783', '42', '0', '0');
INSERT INTO `hotkeys` VALUES ('453', '8783', '43', '0', '0');
INSERT INTO `hotkeys` VALUES ('454', '8783', '44', '0', '0');
INSERT INTO `hotkeys` VALUES ('455', '8783', '45', '0', '0');
INSERT INTO `hotkeys` VALUES ('456', '8783', '46', '0', '0');
INSERT INTO `hotkeys` VALUES ('457', '8783', '47', '0', '0');
INSERT INTO `hotkeys` VALUES ('458', '8783', '48', '0', '0');
INSERT INTO `hotkeys` VALUES ('459', '8783', '49', '0', '0');
INSERT INTO `hotkeys` VALUES ('460', '8783', '50', '0', '0');
INSERT INTO `hotkeys` VALUES ('461', '8785', '0', '0', '0');
INSERT INTO `hotkeys` VALUES ('462', '8785', '1', '0', '0');
INSERT INTO `hotkeys` VALUES ('463', '8785', '2', '0', '0');
INSERT INTO `hotkeys` VALUES ('464', '8785', '3', '0', '0');
INSERT INTO `hotkeys` VALUES ('465', '8785', '4', '0', '0');
INSERT INTO `hotkeys` VALUES ('466', '8785', '5', '0', '0');
INSERT INTO `hotkeys` VALUES ('467', '8785', '6', '0', '0');
INSERT INTO `hotkeys` VALUES ('468', '8785', '7', '0', '0');
INSERT INTO `hotkeys` VALUES ('469', '8785', '8', '0', '0');
INSERT INTO `hotkeys` VALUES ('470', '8785', '9', '0', '0');
INSERT INTO `hotkeys` VALUES ('471', '8785', '10', '0', '0');
INSERT INTO `hotkeys` VALUES ('472', '8785', '11', '0', '0');
INSERT INTO `hotkeys` VALUES ('473', '8785', '12', '0', '0');
INSERT INTO `hotkeys` VALUES ('474', '8785', '13', '0', '0');
INSERT INTO `hotkeys` VALUES ('475', '8785', '14', '0', '0');
INSERT INTO `hotkeys` VALUES ('476', '8785', '15', '0', '0');
INSERT INTO `hotkeys` VALUES ('477', '8785', '16', '0', '0');
INSERT INTO `hotkeys` VALUES ('478', '8785', '17', '0', '0');
INSERT INTO `hotkeys` VALUES ('479', '8785', '18', '0', '0');
INSERT INTO `hotkeys` VALUES ('480', '8785', '19', '0', '0');
INSERT INTO `hotkeys` VALUES ('481', '8785', '20', '0', '0');
INSERT INTO `hotkeys` VALUES ('482', '8785', '21', '0', '0');
INSERT INTO `hotkeys` VALUES ('483', '8785', '22', '0', '0');
INSERT INTO `hotkeys` VALUES ('484', '8785', '23', '0', '0');
INSERT INTO `hotkeys` VALUES ('485', '8785', '24', '0', '0');
INSERT INTO `hotkeys` VALUES ('486', '8785', '25', '0', '0');
INSERT INTO `hotkeys` VALUES ('487', '8785', '26', '0', '0');
INSERT INTO `hotkeys` VALUES ('488', '8785', '27', '0', '0');
INSERT INTO `hotkeys` VALUES ('489', '8785', '28', '0', '0');
INSERT INTO `hotkeys` VALUES ('490', '8785', '29', '0', '0');
INSERT INTO `hotkeys` VALUES ('491', '8785', '30', '0', '0');
INSERT INTO `hotkeys` VALUES ('492', '8785', '31', '0', '0');
INSERT INTO `hotkeys` VALUES ('493', '8785', '32', '0', '0');
INSERT INTO `hotkeys` VALUES ('494', '8785', '33', '0', '0');
INSERT INTO `hotkeys` VALUES ('495', '8785', '34', '0', '0');
INSERT INTO `hotkeys` VALUES ('496', '8785', '35', '0', '0');
INSERT INTO `hotkeys` VALUES ('497', '8785', '36', '0', '0');
INSERT INTO `hotkeys` VALUES ('498', '8785', '37', '0', '0');
INSERT INTO `hotkeys` VALUES ('499', '8785', '38', '0', '0');
INSERT INTO `hotkeys` VALUES ('500', '8785', '39', '0', '0');
INSERT INTO `hotkeys` VALUES ('501', '8785', '40', '0', '0');
INSERT INTO `hotkeys` VALUES ('502', '8785', '41', '0', '0');
INSERT INTO `hotkeys` VALUES ('503', '8785', '42', '0', '0');
INSERT INTO `hotkeys` VALUES ('504', '8785', '43', '0', '0');
INSERT INTO `hotkeys` VALUES ('505', '8785', '44', '0', '0');
INSERT INTO `hotkeys` VALUES ('506', '8785', '45', '0', '0');
INSERT INTO `hotkeys` VALUES ('507', '8785', '46', '0', '0');
INSERT INTO `hotkeys` VALUES ('508', '8785', '47', '0', '0');
INSERT INTO `hotkeys` VALUES ('509', '8785', '48', '0', '0');
INSERT INTO `hotkeys` VALUES ('510', '8785', '49', '0', '0');
INSERT INTO `hotkeys` VALUES ('511', '8785', '50', '0', '0');
INSERT INTO `hotkeys` VALUES ('512', '8840', '0', '0', '0');
INSERT INTO `hotkeys` VALUES ('513', '8840', '1', '0', '0');
INSERT INTO `hotkeys` VALUES ('514', '8840', '2', '0', '0');
INSERT INTO `hotkeys` VALUES ('515', '8840', '3', '0', '0');
INSERT INTO `hotkeys` VALUES ('516', '8840', '4', '0', '0');
INSERT INTO `hotkeys` VALUES ('517', '8840', '5', '0', '0');
INSERT INTO `hotkeys` VALUES ('518', '8840', '6', '0', '0');
INSERT INTO `hotkeys` VALUES ('519', '8840', '7', '0', '0');
INSERT INTO `hotkeys` VALUES ('520', '8840', '8', '0', '0');
INSERT INTO `hotkeys` VALUES ('521', '8840', '9', '0', '0');
INSERT INTO `hotkeys` VALUES ('522', '8840', '10', '0', '0');
INSERT INTO `hotkeys` VALUES ('523', '8840', '11', '0', '0');
INSERT INTO `hotkeys` VALUES ('524', '8840', '12', '0', '0');
INSERT INTO `hotkeys` VALUES ('525', '8840', '13', '0', '0');
INSERT INTO `hotkeys` VALUES ('526', '8840', '14', '0', '0');
INSERT INTO `hotkeys` VALUES ('527', '8840', '15', '0', '0');
INSERT INTO `hotkeys` VALUES ('528', '8840', '16', '0', '0');
INSERT INTO `hotkeys` VALUES ('529', '8840', '17', '0', '0');
INSERT INTO `hotkeys` VALUES ('530', '8840', '18', '0', '0');
INSERT INTO `hotkeys` VALUES ('531', '8840', '19', '0', '0');
INSERT INTO `hotkeys` VALUES ('532', '8840', '20', '0', '0');
INSERT INTO `hotkeys` VALUES ('533', '8840', '21', '0', '0');
INSERT INTO `hotkeys` VALUES ('534', '8840', '22', '0', '0');
INSERT INTO `hotkeys` VALUES ('535', '8840', '23', '0', '0');
INSERT INTO `hotkeys` VALUES ('536', '8840', '24', '0', '0');
INSERT INTO `hotkeys` VALUES ('537', '8840', '25', '0', '0');
INSERT INTO `hotkeys` VALUES ('538', '8840', '26', '0', '0');
INSERT INTO `hotkeys` VALUES ('539', '8840', '27', '0', '0');
INSERT INTO `hotkeys` VALUES ('540', '8840', '28', '0', '0');
INSERT INTO `hotkeys` VALUES ('541', '8840', '29', '0', '0');
INSERT INTO `hotkeys` VALUES ('542', '8840', '30', '0', '0');
INSERT INTO `hotkeys` VALUES ('543', '8840', '31', '0', '0');
INSERT INTO `hotkeys` VALUES ('544', '8840', '32', '0', '0');
INSERT INTO `hotkeys` VALUES ('545', '8840', '33', '0', '0');
INSERT INTO `hotkeys` VALUES ('546', '8840', '34', '0', '0');
INSERT INTO `hotkeys` VALUES ('547', '8840', '35', '0', '0');
INSERT INTO `hotkeys` VALUES ('548', '8840', '36', '0', '0');
INSERT INTO `hotkeys` VALUES ('549', '8840', '37', '0', '0');
INSERT INTO `hotkeys` VALUES ('550', '8840', '38', '0', '0');
INSERT INTO `hotkeys` VALUES ('551', '8840', '39', '0', '0');
INSERT INTO `hotkeys` VALUES ('552', '8840', '40', '0', '0');
INSERT INTO `hotkeys` VALUES ('553', '8840', '41', '0', '0');
INSERT INTO `hotkeys` VALUES ('554', '8840', '42', '0', '0');
INSERT INTO `hotkeys` VALUES ('555', '8840', '43', '0', '0');
INSERT INTO `hotkeys` VALUES ('556', '8840', '44', '0', '0');
INSERT INTO `hotkeys` VALUES ('557', '8840', '45', '0', '0');
INSERT INTO `hotkeys` VALUES ('558', '8840', '46', '0', '0');
INSERT INTO `hotkeys` VALUES ('559', '8840', '47', '0', '0');
INSERT INTO `hotkeys` VALUES ('560', '8840', '48', '0', '0');
INSERT INTO `hotkeys` VALUES ('561', '8840', '49', '0', '0');
INSERT INTO `hotkeys` VALUES ('562', '8840', '50', '0', '0');
INSERT INTO `items` VALUES ('1159', '0', '1', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('1160', '3640', '1', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('1161', '0', '1', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('1162', '0', '1', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('1163', '3641', '1', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('1164', '3642', '1', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('1165', '0', '1', '0', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('1166', '0', '1', '0', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('1167', '0', '1', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('1168', '0', '1', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('1169', '0', '1', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('1170', '0', '1', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('1171', '0', '1', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('1172', '3851', '1', '0', '13', '8', '30', 'item13');
INSERT INTO `items` VALUES ('1173', '8985', '1', '0', '14', '1', '30', 'item14');
INSERT INTO `items` VALUES ('1174', '3829', '1', '0', '15', '1', '30', 'item15');
INSERT INTO `items` VALUES ('1175', '3795', '1', '0', '16', '10', '30', 'item16');
INSERT INTO `items` VALUES ('1176', '3769', '1', '0', '17', '11', '30', 'item17');
INSERT INTO `items` VALUES ('1177', '5912', '1', '0', '18', '998', '30', 'item18');
INSERT INTO `items` VALUES ('1178', '5913', '1', '0', '19', '998', '30', 'item19');
INSERT INTO `items` VALUES ('1179', '3823', '1', '0', '20', '10000', '30', 'item20');
INSERT INTO `items` VALUES ('1180', '24405', '1', '0', '21', '1', '30', 'item21');
INSERT INTO `items` VALUES ('1181', '24406', '1', '0', '22', '1', '30', 'item22');
INSERT INTO `items` VALUES ('1182', '23297', '1', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('1183', '23299', '1', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('1184', '3635', '1', '1', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('1185', '24311', '1', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('1186', '24286', '1', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('1187', '3784', '1', '0', '28', '11', '30', 'item28');
INSERT INTO `items` VALUES ('1188', '8', '1', '0', '29', '45', '30', 'item29');
INSERT INTO `items` VALUES ('1189', '8', '1', '0', '30', '49', '30', 'item30');
INSERT INTO `items` VALUES ('1190', '8', '1', '0', '31', '50', '30', 'item31');
INSERT INTO `items` VALUES ('1191', '8', '1', '0', '32', '50', '30', 'item32');
INSERT INTO `items` VALUES ('1192', '3851', '1', '0', '33', '10', '30', 'item33');
INSERT INTO `items` VALUES ('1193', '19634', '1', '5', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('1194', '3795', '1', '0', '35', '11', '30', 'item35');
INSERT INTO `items` VALUES ('1195', '3640', '1', '2', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('1196', '0', '1', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('1197', '0', '1', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('1198', '3642', '1', '2', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('1199', '3641', '1', '2', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('1200', '0', '1', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('1201', '0', '1', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('1202', '0', '1', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('1203', '3635', '1', '1', '44', '0', '30', 'item44');
INSERT INTO `items` VALUES ('1204', '0', '8780', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('1205', '3640', '8780', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('1206', '0', '8780', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('1207', '0', '8780', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('1208', '3641', '8780', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('1209', '3642', '8780', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('1210', '3634', '8780', '1', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('1211', '0', '8780', '0', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('1212', '0', '8780', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('1213', '0', '8780', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('1214', '0', '8780', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('1215', '0', '8780', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('1216', '0', '8780', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('1217', '0', '8780', '0', '13', '0', '30', 'item13');
INSERT INTO `items` VALUES ('1218', '0', '8780', '0', '14', '0', '30', 'item14');
INSERT INTO `items` VALUES ('1219', '0', '8780', '0', '15', '0', '30', 'item15');
INSERT INTO `items` VALUES ('1220', '0', '8780', '0', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('1221', '0', '8780', '0', '17', '0', '30', 'item17');
INSERT INTO `items` VALUES ('1222', '0', '8780', '0', '18', '0', '30', 'item18');
INSERT INTO `items` VALUES ('1223', '0', '8780', '0', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('1224', '0', '8780', '0', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('1225', '0', '8780', '0', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('1226', '0', '8780', '0', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('1227', '0', '8780', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('1228', '0', '8780', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('1229', '0', '8780', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('1230', '0', '8780', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('1231', '0', '8780', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('1232', '0', '8780', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('1233', '0', '8780', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('1234', '0', '8780', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('1235', '0', '8780', '0', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('1236', '0', '8780', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('1237', '0', '8780', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('1238', '0', '8780', '0', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('1239', '0', '8780', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('1240', '0', '8780', '0', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('1241', '0', '8780', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('1242', '0', '8780', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('1243', '0', '8780', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('1244', '0', '8780', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('1245', '0', '8780', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('1246', '0', '8780', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('1247', '0', '8780', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('1248', '0', '8780', '0', '44', '0', '30', 'item44');
INSERT INTO `items` VALUES ('1249', '0', '8781', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('1250', '11474', '8781', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('1251', '0', '8781', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('1252', '0', '8781', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('1253', '11475', '8781', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('1254', '11476', '8781', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('1255', '10735', '8781', '1', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('1256', '0', '8781', '0', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('1257', '0', '8781', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('1258', '0', '8781', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('1259', '0', '8781', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('1260', '0', '8781', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('1261', '0', '8781', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('1262', '3851', '8781', '0', '13', '10', '30', 'item13');
INSERT INTO `items` VALUES ('1263', '0', '8781', '0', '14', '0', '30', 'item14');
INSERT INTO `items` VALUES ('1264', '0', '8781', '0', '15', '0', '30', 'item15');
INSERT INTO `items` VALUES ('1265', '0', '8781', '0', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('1266', '0', '8781', '0', '17', '0', '30', 'item17');
INSERT INTO `items` VALUES ('1267', '0', '8781', '0', '18', '0', '30', 'item18');
INSERT INTO `items` VALUES ('1268', '0', '8781', '0', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('1269', '0', '8781', '0', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('1270', '0', '8781', '0', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('1271', '0', '8781', '0', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('1272', '0', '8781', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('1273', '0', '8781', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('1274', '0', '8781', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('1275', '0', '8781', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('1276', '0', '8781', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('1277', '0', '8781', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('1278', '0', '8781', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('1279', '0', '8781', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('1280', '0', '8781', '0', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('1281', '0', '8781', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('1282', '0', '8781', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('1283', '0', '8781', '0', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('1284', '0', '8781', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('1285', '0', '8781', '0', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('1286', '0', '8781', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('1287', '0', '8781', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('1288', '0', '8781', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('1289', '0', '8781', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('1290', '0', '8781', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('1291', '0', '8781', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('1292', '0', '8781', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('1293', '0', '8781', '0', '44', '0', '30', 'item44');
INSERT INTO `items` VALUES ('1294', '0', '8782', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('1295', '3637', '8782', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('1296', '0', '8782', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('1297', '0', '8782', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('1298', '3638', '8782', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('1299', '3639', '8782', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('1300', '3632', '8782', '1', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('1301', '251', '8782', '3', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('1302', '0', '8782', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('1303', '0', '8782', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('1304', '0', '8782', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('1305', '0', '8782', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('1306', '0', '8782', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('1307', '3795', '8782', '0', '13', '11', '30', 'item13');
INSERT INTO `items` VALUES ('1308', '3829', '8782', '0', '14', '1', '30', 'item14');
INSERT INTO `items` VALUES ('1309', '0', '8782', '0', '15', '0', '30', 'item15');
INSERT INTO `items` VALUES ('1310', '0', '8782', '0', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('1311', '0', '8782', '0', '17', '0', '30', 'item17');
INSERT INTO `items` VALUES ('1312', '0', '8782', '0', '18', '0', '30', 'item18');
INSERT INTO `items` VALUES ('1313', '0', '8782', '0', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('1314', '0', '8782', '0', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('1315', '0', '8782', '0', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('1316', '0', '8782', '0', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('1317', '0', '8782', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('1318', '0', '8782', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('1319', '0', '8782', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('1320', '0', '8782', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('1321', '0', '8782', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('1322', '0', '8782', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('1323', '0', '8782', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('1324', '0', '8782', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('1325', '0', '8782', '0', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('1326', '0', '8782', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('1327', '0', '8782', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('1328', '0', '8782', '0', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('1329', '0', '8782', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('1330', '0', '8782', '0', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('1331', '0', '8782', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('1332', '0', '8782', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('1333', '0', '8782', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('1334', '0', '8782', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('1335', '0', '8782', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('1336', '0', '8782', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('1337', '0', '8782', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('1338', '0', '8782', '0', '44', '0', '30', 'item44');
INSERT INTO `items` VALUES ('1339', '0', '8783', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('1340', '3640', '8783', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('1341', '0', '8783', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('1342', '0', '8783', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('1343', '3641', '8783', '0', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('1344', '3642', '8783', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('1345', '3635', '8783', '4', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('1346', '0', '8783', '0', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('1347', '0', '8783', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('1348', '0', '8783', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('1349', '0', '8783', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('1350', '0', '8783', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('1351', '0', '8783', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('1352', '0', '8783', '0', '13', '0', '30', 'item13');
INSERT INTO `items` VALUES ('1353', '0', '8783', '0', '14', '0', '30', 'item14');
INSERT INTO `items` VALUES ('1354', '0', '8783', '0', '15', '0', '30', 'item15');
INSERT INTO `items` VALUES ('1355', '0', '8783', '0', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('1356', '0', '8783', '0', '17', '0', '30', 'item17');
INSERT INTO `items` VALUES ('1357', '0', '8783', '0', '18', '0', '30', 'item18');
INSERT INTO `items` VALUES ('1358', '0', '8783', '0', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('1359', '0', '8783', '0', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('1360', '0', '8783', '0', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('1361', '0', '8783', '0', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('1362', '0', '8783', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('1363', '0', '8783', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('1364', '0', '8783', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('1365', '0', '8783', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('1366', '0', '8783', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('1367', '0', '8783', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('1368', '0', '8783', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('1369', '0', '8783', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('1370', '0', '8783', '0', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('1371', '0', '8783', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('1372', '0', '8783', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('1373', '0', '8783', '0', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('1374', '0', '8783', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('1375', '0', '8783', '0', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('1376', '0', '8783', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('1377', '0', '8783', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('1378', '0', '8783', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('1379', '0', '8783', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('1380', '0', '8783', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('1381', '0', '8783', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('1382', '0', '8783', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('1383', '0', '8783', '0', '44', '0', '30', 'item44');
INSERT INTO `items` VALUES ('1384', '0', '8785', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('1385', '3637', '8785', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('1386', '0', '8785', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('1387', '0', '8785', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('1388', '3638', '8785', '0', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('1389', '3639', '8785', '1', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('1390', '3632', '8785', '3', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('1391', '251', '8785', '4', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('1392', '0', '8785', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('1393', '0', '8785', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('1394', '0', '8785', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('1395', '0', '8785', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('1396', '0', '8785', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('1397', '0', '8785', '0', '13', '0', '30', 'item13');
INSERT INTO `items` VALUES ('1398', '0', '8785', '0', '14', '0', '30', 'item14');
INSERT INTO `items` VALUES ('1399', '0', '8785', '0', '15', '0', '30', 'item15');
INSERT INTO `items` VALUES ('1400', '0', '8785', '0', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('1401', '0', '8785', '0', '17', '0', '30', 'item17');
INSERT INTO `items` VALUES ('1402', '0', '8785', '0', '18', '0', '30', 'item18');
INSERT INTO `items` VALUES ('1403', '0', '8785', '0', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('1404', '0', '8785', '0', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('1405', '0', '8785', '0', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('1406', '0', '8785', '0', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('1407', '0', '8785', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('1408', '0', '8785', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('1409', '0', '8785', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('1410', '0', '8785', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('1411', '0', '8785', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('1412', '0', '8785', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('1413', '0', '8785', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('1414', '0', '8785', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('1415', '0', '8785', '0', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('1416', '0', '8785', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('1417', '0', '8785', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('1418', '0', '8785', '0', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('1419', '0', '8785', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('1420', '0', '8785', '0', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('1421', '0', '8785', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('1422', '0', '8785', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('1423', '0', '8785', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('1424', '0', '8785', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('1425', '0', '8785', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('1426', '0', '8785', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('1427', '0', '8785', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('1428', '0', '8785', '0', '44', '0', '30', 'item44');
INSERT INTO `items` VALUES ('1429', '0', '8840', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('1430', '0', '8840', '0', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('1431', '0', '8840', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('1432', '0', '8840', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('1433', '3644', '8840', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('1434', '3645', '8840', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('1435', '3636', '8840', '1', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('1436', '62', '8840', '0', '7', '100', '30', 'item7');
INSERT INTO `items` VALUES ('1437', '0', '8840', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('1438', '0', '8840', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('1439', '0', '8840', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('1440', '0', '8840', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('1441', '0', '8840', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('1442', '23297', '8840', '0', '13', '0', '30', 'item13');
INSERT INTO `items` VALUES ('1443', '22622', '8840', '0', '14', '0', '30', 'item14');
INSERT INTO `items` VALUES ('1444', '3795', '8840', '0', '15', '11', '30', 'item15');
INSERT INTO `items` VALUES ('1445', '3851', '8840', '0', '16', '11', '30', 'item16');
INSERT INTO `items` VALUES ('1446', '3769', '8840', '0', '17', '10', '30', 'item17');
INSERT INTO `items` VALUES ('1447', '0', '8840', '0', '18', '0', '30', 'item18');
INSERT INTO `items` VALUES ('1448', '0', '8840', '0', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('1449', '0', '8840', '0', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('1450', '0', '8840', '0', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('1451', '0', '8840', '0', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('1452', '0', '8840', '0', '23', '0', '0', 'item23');
INSERT INTO `items` VALUES ('1453', '0', '8840', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('1454', '0', '8840', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('1455', '0', '8840', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('1456', '0', '8840', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('1457', '0', '8840', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('1458', '0', '8840', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('1459', '0', '8840', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('1460', '0', '8840', '0', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('1461', '0', '8840', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('1462', '0', '8840', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('1463', '0', '8840', '0', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('1464', '0', '8840', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('1465', '0', '8840', '0', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('1466', '0', '8840', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('1467', '0', '8840', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('1468', '0', '8840', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('1469', '0', '8840', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('1470', '0', '8840', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('1471', '0', '8840', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('1472', '0', '8840', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('1473', '0', '8840', '0', '44', '0', '30', 'item44');
INSERT INTO `log` VALUES ('1', '0000-00-00 00:00:00', '127.0.0.1:59141', '[UNKNOWN]', 'Login', 'Sucess', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('2', '0000-00-00 00:00:00', '127.0.0.1:59143', '[UNKNOWN]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('3', '0000-00-00 00:00:00', '127.0.0.1:59143', '[UNKNOWN]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('4', '0000-00-00 00:00:00', '127.0.0.1:59150', '[UNKNOWN]', 'Login', 'Sucess', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('5', '0000-00-00 00:00:00', '127.0.0.1:59696', '[UNKNOWN]', 'Login', 'Sucess', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('6', '0000-00-00 00:00:00', '127.0.0.1:59805', '[UNKNOWN]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('7', '2010-11-01 19:00:54', '127.0.0.1:59832', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('8', '2010-11-01 19:02:30', '127.0.0.1:59832', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('9', '2010-11-06 16:08:44', '84.157.132.93:52556', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('10', '2010-11-06 16:08:51', '84.157.132.93:52556', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('11', '2010-11-14 18:13:18', '84.157.131.234:49579', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('12', '2010-11-14 18:13:25', '84.157.131.234:49579', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('13', '2010-11-14 18:13:36', '84.157.131.234:49581', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('14', '2010-11-14 18:13:43', '84.157.131.234:49581', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('15', '2010-11-14 18:17:39', '84.157.131.234:49588', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('16', '2010-11-14 18:17:44', '84.157.131.234:49588', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('17', '2010-11-14 18:22:58', '84.157.131.234:49598', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('18', '2010-11-14 18:23:15', '84.157.131.234:49598', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('19', '2010-11-14 18:24:07', '84.157.131.234:49600', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('20', '2010-11-14 18:24:34', '84.157.131.234:49600', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('21', '2010-11-14 18:27:01', '84.157.131.234:49603', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('22', '2010-11-14 18:27:08', '84.157.131.234:49603', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('23', '2010-11-14 18:35:24', '84.157.131.234:49644', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('24', '2010-11-14 18:35:31', '84.157.131.234:49644', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('25', '2010-11-14 18:36:37', '84.157.131.234:49645', 'Over', 'GM', 'Custom_Command', 'Command: \\stat 100');
INSERT INTO `log` VALUES ('26', '2010-11-14 18:36:37', '84.157.131.234:49645', 'Over', 'Chat', 'GM', 'Message: \\stat 100');
INSERT INTO `log` VALUES ('27', '2010-11-14 18:43:46', '84.157.131.234:49652', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('28', '2010-11-14 18:47:48', '84.157.131.234:49652', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('29', '2010-11-14 18:56:20', '84.157.131.234:50023', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('30', '2010-11-14 18:57:29', '84.157.131.234:50024', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('31', '2010-11-14 18:57:37', '84.157.131.234:50024', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('32', '2010-11-14 19:45:43', '84.157.131.234:50326', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('33', '2010-11-14 19:45:58', '84.157.131.234:50326', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('34', '2010-11-14 19:46:42', '84.157.131.234:50328', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('35', '2010-11-14 19:47:30', '84.157.131.234:50328', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('36', '2010-11-14 19:49:54', '84.157.131.234:50329', 'Over', 'GM', 'Item_Create', 'Slot:29, ID:8, Dura:30, Amout:50, Plus:0');
INSERT INTO `log` VALUES ('37', '2010-11-14 19:49:55', '84.157.131.234:50329', 'Over', 'GM', 'Item_Create', 'Slot:30, ID:8, Dura:30, Amout:50, Plus:0');
INSERT INTO `log` VALUES ('38', '2010-11-14 19:49:56', '84.157.131.234:50329', 'Over', 'GM', 'Item_Create', 'Slot:31, ID:8, Dura:30, Amout:50, Plus:0');
INSERT INTO `log` VALUES ('39', '2010-11-14 19:49:56', '84.157.131.234:50329', 'Over', 'GM', 'Item_Create', 'Slot:32, ID:8, Dura:30, Amout:50, Plus:0');
INSERT INTO `log` VALUES ('40', '2010-11-14 19:50:11', '84.197.104.142:1972', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('41', '2010-11-14 19:51:36', '84.197.104.142:1972', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('42', '2010-11-14 19:52:03', '84.157.131.234:50329', 'Over', 'Chat', 'Global', 'Message: THere?');
INSERT INTO `log` VALUES ('43', '2010-11-14 19:52:08', '84.197.104.142:2025', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('44', '2010-11-14 19:52:30', '84.197.104.142:2025', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('45', '2010-11-14 19:53:37', '84.157.131.234:50329', 'Over', 'Chat', 'Global', 'Message: there?');
INSERT INTO `log` VALUES ('46', '2010-11-14 19:53:48', '84.197.104.142:2043', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('47', '2010-11-14 19:53:53', '84.157.131.234:50329', 'Over', 'Chat', 'Global', 'Message: just buy a global on item mall');
INSERT INTO `log` VALUES ('48', '2010-11-14 19:54:00', '84.197.104.142:2043', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('49', '2010-11-14 19:54:46', '84.197.104.142:2046', '___', 'GM', 'Custom_Command', 'Command: ~~');
INSERT INTO `log` VALUES ('50', '2010-11-14 19:54:46', '84.197.104.142:2046', '___', 'Chat', 'GM', 'Message: ~~');
INSERT INTO `log` VALUES ('51', '2010-11-14 19:55:35', '84.157.131.234:50333', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('52', '2010-11-14 19:56:23', '84.157.131.234:50333', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('53', '2010-11-14 19:56:50', '84.157.131.234:50334', 'Over', 'Chat', 'Notice', 'Message: where u');
INSERT INTO `log` VALUES ('54', '2010-11-14 19:58:44', '84.157.131.234:50338', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('55', '2010-11-14 19:58:51', '84.157.131.234:50338', '[UNKNWON]', 'Register', '(None)', 'Name: new, Password: new');
INSERT INTO `log` VALUES ('56', '2010-11-14 19:59:04', '84.157.131.234:50339', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('57', '2010-11-14 19:59:11', '84.157.131.234:50339', '[UNKNWON]', 'Register', '(None)', 'Name: new , Password: new');
INSERT INTO `log` VALUES ('58', '2010-11-14 19:59:15', '84.157.131.234:50339', '[UNKNWON]', 'Login', 'Sucess', 'Name: new, Server: Remote');
INSERT INTO `log` VALUES ('59', '2010-11-14 20:06:38', '84.197.104.142:2270', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('60', '2010-11-14 20:06:45', '84.197.104.142:2270', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('61', '2010-11-15 17:01:02', '84.197.104.142:13347', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('62', '2010-11-15 17:01:17', '84.197.104.142:13347', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('63', '2010-11-15 17:05:32', '84.197.104.142:13351', '___', 'Chat', 'Global', 'Message: YAY!!');
INSERT INTO `log` VALUES ('64', '2010-11-15 17:12:36', '84.197.104.142:13351', '___', 'GM', 'Custom_Command', 'Command: ___');
INSERT INTO `log` VALUES ('65', '2010-11-15 17:12:36', '84.197.104.142:13351', '___', 'Chat', 'GM', 'Message: ___');
INSERT INTO `log` VALUES ('66', '2010-11-15 17:12:39', '84.197.104.142:13351', '___', 'GM', 'Custom_Command', 'Command: __');
INSERT INTO `log` VALUES ('67', '2010-11-15 17:12:39', '84.197.104.142:13351', '___', 'Chat', 'GM', 'Message: __');
INSERT INTO `log` VALUES ('68', '2010-11-15 17:12:41', '84.197.104.142:13351', '___', 'GM', 'Custom_Command', 'Command: ___');
INSERT INTO `log` VALUES ('69', '2010-11-15 17:12:41', '84.197.104.142:13351', '___', 'Chat', 'GM', 'Message: ___');
INSERT INTO `log` VALUES ('70', '2010-11-17 20:19:55', '201.242.159.129:50839', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('71', '2010-11-17 20:20:26', '201.242.159.129:50839', '[UNKNWON]', 'Register', '(None)', 'Name: eckonet, Password: 1234567891');
INSERT INTO `log` VALUES ('72', '2010-11-17 20:21:00', '201.242.159.129:50840', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('73', '2010-11-17 20:21:13', '201.242.159.129:50840', '[UNKNWON]', 'Login', 'Sucess', 'Name: eckonet, Server: Remote');
INSERT INTO `log` VALUES ('74', '2010-11-17 20:23:31', '201.242.159.129:50845', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('75', '2010-11-17 20:23:33', '201.242.159.129:50844', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('76', '2010-11-17 20:23:42', '201.242.159.129:50845', '[UNKNWON]', 'Login', 'Sucess', 'Name: eckonet, Server: Remote');
INSERT INTO `log` VALUES ('77', '2010-11-17 20:24:45', '201.242.159.129:50844', '[UNKNWON]', 'Login', 'Sucess', 'Name: eckonet, Server: Remote');
INSERT INTO `log` VALUES ('78', '2010-11-17 20:26:09', '201.242.159.129:50848', '_1_2_3_4_5_', 'GM', 'Custom_Command', 'Command: 2');
INSERT INTO `log` VALUES ('79', '2010-11-17 20:26:09', '201.242.159.129:50848', '_1_2_3_4_5_', 'Chat', 'GM', 'Message: 2');
INSERT INTO `log` VALUES ('80', '2010-11-17 20:48:00', '201.242.159.129:50913', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('81', '2010-11-17 20:48:13', '201.242.159.129:50913', '[UNKNWON]', 'Login', 'Sucess', 'Name: eckonet, Server: Remote');
INSERT INTO `log` VALUES ('82', '2010-11-17 20:48:37', '201.242.159.129:50915', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('83', '2010-11-17 20:48:52', '201.242.159.129:50915', '[UNKNWON]', 'Login', 'Sucess', 'Name: eckonet, Server: Remote');
INSERT INTO `log` VALUES ('84', '2010-11-17 20:49:33', '201.242.159.129:50922', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('85', '2010-11-17 20:49:47', '201.242.159.129:50922', '[UNKNWON]', 'Login', 'Sucess', 'Name: eckonet, Server: Remote');
INSERT INTO `log` VALUES ('86', '2010-11-19 02:45:22', '201.242.159.129:51201', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('87', '2010-11-22 21:32:53', '84.157.144.77:49769', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('88', '2010-11-22 21:33:23', '84.157.144.77:49769', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('89', '2010-11-22 21:33:33', '84.157.144.77:49771', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('90', '2010-11-22 21:34:05', '84.157.144.77:49771', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('91', '2010-11-22 21:41:41', '84.157.144.77:50331', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('92', '2010-11-22 21:44:38', '84.157.144.77:50331', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('93', '2010-11-24 16:46:50', '84.157.137.228:55761', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('94', '2010-11-24 16:47:02', '84.157.137.228:55761', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('95', '2010-11-24 16:48:52', '84.157.137.228:55930', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('96', '2010-11-24 16:49:13', '84.157.137.228:55930', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('97', '2010-11-24 16:51:23', '84.157.137.228:55931', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 14778, Monster_Name: MOB_AM_IVY Type: 3');
INSERT INTO `log` VALUES ('98', '2010-11-24 16:51:35', '84.157.137.228:56043', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('99', '2010-11-24 16:51:43', '84.157.137.228:56043', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('100', '2010-11-24 16:57:26', '84.197.104.142:3258', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('101', '2010-11-24 16:58:08', '84.197.104.142:3258', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('102', '2010-11-24 16:58:37', '84.197.104.142:3270', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('103', '2010-11-24 16:59:24', '84.157.137.228:56220', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('104', '2010-11-24 16:59:39', '84.157.137.228:56220', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('105', '2010-11-24 16:59:46', '84.197.104.142:3270', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('106', '2010-11-24 17:01:08', '84.197.104.142:3290', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('107', '2010-11-24 17:01:17', '84.157.137.228:56222', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('108', '2010-11-24 17:01:20', '84.157.137.228:56222', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('109', '2010-11-24 17:01:45', '84.157.137.228:56224', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('110', '2010-11-24 17:01:49', '84.157.137.228:56224', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('111', '2010-11-24 17:02:23', '84.157.137.228:56227', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('112', '2010-11-24 17:02:31', '84.157.137.228:56227', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('113', '2010-11-24 17:02:32', '84.197.104.142:3300', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('114', '2010-11-24 17:02:50', '84.197.104.142:3300', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('115', '2010-11-24 17:03:33', '84.157.137.228:56228', 'Over', 'GM', 'Custom_Command', 'Command: so');
INSERT INTO `log` VALUES ('116', '2010-11-24 17:03:33', '84.157.137.228:56228', 'Over', 'Chat', 'GM', 'Message: so');
INSERT INTO `log` VALUES ('117', '2010-11-24 17:04:02', '84.157.137.228:56233', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('118', '2010-11-24 17:04:14', '84.157.137.228:56233', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('119', '2010-11-24 17:04:21', '84.197.104.142:3320', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('120', '2010-11-24 17:04:59', '84.197.104.142:3320', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('121', '2010-11-24 17:06:20', '84.157.137.228:56234', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('122', '2010-11-24 17:07:31', '84.157.137.228:56237', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('123', '2010-11-24 17:07:47', '84.157.137.228:56237', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('124', '2010-11-24 17:14:31', '84.157.137.228:56401', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('125', '2010-11-24 17:14:55', '84.157.137.228:56401', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('126', '2010-11-24 17:16:15', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 0');
INSERT INTO `log` VALUES ('127', '2010-11-24 17:16:19', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 0');
INSERT INTO `log` VALUES ('128', '2010-11-24 17:16:22', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 0');
INSERT INTO `log` VALUES ('129', '2010-11-24 17:16:24', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 0');
INSERT INTO `log` VALUES ('130', '2010-11-24 17:16:27', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 0');
INSERT INTO `log` VALUES ('131', '2010-11-24 17:16:29', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 0');
INSERT INTO `log` VALUES ('132', '2010-11-24 17:16:31', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 0');
INSERT INTO `log` VALUES ('133', '2010-11-24 17:16:33', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 0');
INSERT INTO `log` VALUES ('134', '2010-11-24 17:16:36', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 0');
INSERT INTO `log` VALUES ('135', '2010-11-24 17:16:38', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 0');
INSERT INTO `log` VALUES ('136', '2010-11-24 17:16:41', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 0');
INSERT INTO `log` VALUES ('137', '2010-11-24 17:16:44', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 3');
INSERT INTO `log` VALUES ('138', '2010-11-24 17:17:56', '84.157.137.228:56403', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1947, Monster_Name: MOB_CH_TIGER Type: 3');
INSERT INTO `log` VALUES ('139', '2010-11-24 17:19:21', '84.197.104.142:3431', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('140', '2010-11-24 17:19:31', '84.197.104.142:3431', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('141', '2010-11-24 17:20:40', '84.197.104.142:3441', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('142', '2010-11-24 17:20:55', '84.197.104.142:3441', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('143', '2010-11-24 17:21:40', '84.197.104.142:3449', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('144', '2010-11-24 17:21:44', '84.197.104.142:3449', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('145', '2010-11-24 17:22:23', '84.157.137.228:56413', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('146', '2010-11-24 17:22:24', '84.157.137.228:56414', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('147', '2010-11-24 17:22:31', '84.157.137.228:56414', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('148', '2010-11-24 17:23:00', '84.197.104.142:3486', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('149', '2010-11-24 17:23:12', '84.197.104.142:3486', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('150', '2010-11-24 17:23:34', '84.157.137.228:56413', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('151', '2010-11-24 17:24:03', '84.157.137.228:56423', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('152', '2010-11-24 17:24:14', '84.157.137.228:56423', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('153', '2010-11-24 17:24:29', '84.157.137.228:56425', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('154', '2010-11-24 17:24:52', '84.157.137.228:56425', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('155', '2010-11-24 17:25:07', '84.157.137.228:56433', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('156', '2010-11-24 17:25:28', '84.157.137.228:56433', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('157', '2010-11-24 17:26:09', '84.157.137.228:56434', 'Over', 'GM', 'Custom_Command', 'Command: see yopu');
INSERT INTO `log` VALUES ('158', '2010-11-24 17:26:09', '84.157.137.228:56434', 'Over', 'Chat', 'GM', 'Message: see yopu');
INSERT INTO `log` VALUES ('159', '2010-11-24 17:26:47', '84.197.104.142:3540', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('160', '2010-11-24 17:28:55', '84.197.104.142:3540', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('161', '2010-11-24 17:29:12', '84.157.137.228:56475', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('162', '2010-11-24 17:41:24', '84.157.137.228:56475', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('163', '2010-11-24 17:41:50', '84.157.137.228:56488', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('164', '2010-11-24 17:42:16', '84.157.137.228:56489', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('165', '2010-11-24 17:42:33', '84.157.137.228:56489', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('166', '2010-11-24 17:59:23', '84.157.137.228:57044', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('167', '2010-11-24 17:59:30', '84.157.137.228:57044', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('168', '2010-11-24 17:59:59', '84.157.137.228:57046', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('169', '2010-11-24 18:00:02', '84.157.137.228:57046', '[UNKNWON]', 'Login', 'Sucess', 'Name: test, Server: Remote');
INSERT INTO `log` VALUES ('170', '2010-12-07 17:35:24', '88.68.33.78:2223', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('171', '2010-12-07 17:35:38', '84.157.135.157:59666', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('172', '2010-12-07 17:36:31', '84.157.135.157:59666', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('173', '2010-12-07 17:37:08', '88.68.33.78:2223', '[UNKNWON]', 'Register', '(None)', 'Name: dongdong, Password: 123polizei');
INSERT INTO `log` VALUES ('174', '2010-12-07 17:37:31', '88.68.33.78:2223', '[UNKNWON]', 'Login', 'Sucess', 'Name: dongdong, Server: Remote');
INSERT INTO `log` VALUES ('175', '2010-12-07 17:39:32', '84.157.135.157:59668', 'Over', 'Chat', 'Notice', 'Message: wo bist');
INSERT INTO `log` VALUES ('176', '2010-12-07 17:39:37', '84.157.135.157:59668', 'Over', 'Chat', 'Notice', 'Message: oder wie heißt du?');
INSERT INTO `log` VALUES ('177', '2010-12-07 17:39:48', '88.68.33.78:2239', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('178', '2010-12-07 17:39:54', '84.157.135.157:59680', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('179', '2010-12-07 17:40:02', '88.68.33.78:2239', '[UNKNWON]', 'Login', 'Sucess', 'Name: dongdong, Server: Remote');
INSERT INTO `log` VALUES ('180', '2010-12-07 17:40:03', '84.157.135.157:59680', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('181', '2010-12-07 17:40:33', '84.157.135.157:59681', 'Over', 'GM', 'Custom_Command', 'Command: gm ist aktiviert');
INSERT INTO `log` VALUES ('182', '2010-12-07 17:40:33', '84.157.135.157:59681', 'Over', 'Chat', 'GM', 'Message: gm ist aktiviert');
INSERT INTO `log` VALUES ('183', '2010-12-07 17:40:53', '88.68.33.78:2241', 'dongdong', 'GM', 'Custom_Command', 'Command: :)');
INSERT INTO `log` VALUES ('184', '2010-12-07 17:40:53', '88.68.33.78:2241', 'dongdong', 'Chat', 'GM', 'Message: :)');
INSERT INTO `log` VALUES ('185', '2010-12-07 17:41:28', '84.157.135.157:59681', 'Over', 'GM', 'Custom_Command', 'Command:  /movetouser..');
INSERT INTO `log` VALUES ('186', '2010-12-07 17:41:28', '84.157.135.157:59681', 'Over', 'Chat', 'GM', 'Message:  /movetouser..');
INSERT INTO `log` VALUES ('187', '2010-12-07 17:41:34', '84.157.135.157:59681', 'Over', 'GM', 'Custom_Command', 'Command:  ist cool');
INSERT INTO `log` VALUES ('188', '2010-12-07 17:41:34', '84.157.135.157:59681', 'Over', 'Chat', 'GM', 'Message:  ist cool');
INSERT INTO `log` VALUES ('189', '2010-12-07 17:41:58', '88.68.33.78:2241', 'dongdong', 'GM', 'Custom_Command', 'Command: lol nice ^^');
INSERT INTO `log` VALUES ('190', '2010-12-07 17:41:58', '88.68.33.78:2241', 'dongdong', 'Chat', 'GM', 'Message: lol nice ^^');
INSERT INTO `log` VALUES ('191', '2010-12-07 17:42:00', '84.157.135.157:59681', 'Over', 'GM', 'Custom_Command', 'Command:  /recalluser');
INSERT INTO `log` VALUES ('192', '2010-12-07 17:42:00', '84.157.135.157:59681', 'Over', 'Chat', 'GM', 'Message:  /recalluser');
INSERT INTO `log` VALUES ('193', '2010-12-07 17:42:07', '84.157.135.157:59681', 'Over', 'GM', 'Custom_Command', 'Command:  /wp ist auch gut');
INSERT INTO `log` VALUES ('194', '2010-12-07 17:42:07', '84.157.135.157:59681', 'Over', 'Chat', 'GM', 'Message:  /wp ist auch gut');
INSERT INTO `log` VALUES ('195', '2010-12-07 17:42:18', '84.157.135.157:59681', 'Over', 'GM', 'Custom_Command', 'Command:  ist besser als warp codes');
INSERT INTO `log` VALUES ('196', '2010-12-07 17:42:18', '84.157.135.157:59681', 'Over', 'Chat', 'GM', 'Message:  ist besser als warp codes');
INSERT INTO `log` VALUES ('197', '2010-12-07 17:42:41', '88.68.33.78:2241', 'dongdong', 'GM', 'Custom_Command', 'Command: wie geht das mit dem wp?');
INSERT INTO `log` VALUES ('198', '2010-12-07 17:42:41', '88.68.33.78:2241', 'dongdong', 'Chat', 'GM', 'Message: wie geht das mit dem wp?');
INSERT INTO `log` VALUES ('199', '2010-12-07 17:42:57', '84.157.135.157:59681', 'Over', 'GM', 'Custom_Command', 'Command:  /addwp [name]');
INSERT INTO `log` VALUES ('200', '2010-12-07 17:42:57', '84.157.135.157:59681', 'Over', 'Chat', 'GM', 'Message:  /addwp [name]');
INSERT INTO `log` VALUES ('201', '2010-12-07 17:43:12', '84.157.135.157:59681', 'Over', 'GM', 'Custom_Command', 'Command: dann /wp und dann F4');
INSERT INTO `log` VALUES ('202', '2010-12-07 17:43:12', '84.157.135.157:59681', 'Over', 'Chat', 'GM', 'Message: dann /wp und dann F4');
INSERT INTO `log` VALUES ('203', '2010-12-07 17:43:46', '88.68.33.78:2241', 'dongdong', 'GM', 'Custom_Command', 'Command: dafür mbrauch ich die gm console oder?');
INSERT INTO `log` VALUES ('204', '2010-12-07 17:43:46', '88.68.33.78:2241', 'dongdong', 'Chat', 'GM', 'Message: dafür mbrauch ich die gm console oder?');
INSERT INTO `log` VALUES ('205', '2010-12-07 17:44:33', '84.157.135.157:59681', 'Over', 'GM', 'Custom_Command', 'Command: klappts?');
INSERT INTO `log` VALUES ('206', '2010-12-07 17:44:33', '84.157.135.157:59681', 'Over', 'Chat', 'GM', 'Message: klappts?');
INSERT INTO `log` VALUES ('207', '2010-12-07 17:44:44', '88.68.33.78:2241', 'dongdong', 'GM', 'Custom_Command', 'Command: ich brauch dafür die gm console oder?');
INSERT INTO `log` VALUES ('208', '2010-12-07 17:44:44', '88.68.33.78:2241', 'dongdong', 'Chat', 'GM', 'Message: ich brauch dafür die gm console oder?');
INSERT INTO `log` VALUES ('209', '2010-12-07 17:45:06', '84.157.135.157:59720', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('210', '2010-12-07 17:45:28', '84.157.135.157:59720', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('211', '2010-12-07 17:46:35', '84.157.135.157:59733', 'Over', 'GM', 'Custom_Command', 'Command: item mall tut auch');
INSERT INTO `log` VALUES ('212', '2010-12-07 17:46:35', '84.157.135.157:59733', 'Over', 'Chat', 'GM', 'Message: item mall tut auch');
INSERT INTO `log` VALUES ('213', '2010-12-07 17:47:19', '88.68.33.78:2251', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('214', '2010-12-07 17:47:54', '88.68.33.78:2252', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('215', '2010-12-07 17:47:54', '84.157.135.157:59733', 'Over', 'Chat', 'Global', 'Message: Hallo?');
INSERT INTO `log` VALUES ('216', '2010-12-07 17:49:11', '88.68.33.78:2252', '[UNKNWON]', 'Login', 'Sucess', 'Name: dongdong, Server: Remote');
INSERT INTO `log` VALUES ('217', '2010-12-07 17:49:37', '84.157.135.157:59742', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('218', '2010-12-07 17:50:55', '84.157.135.157:59742', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('219', '2010-12-07 17:51:25', '84.157.135.157:59747', 'Over', 'GM', 'Custom_Command', 'Command: oO');
INSERT INTO `log` VALUES ('220', '2010-12-07 17:51:25', '84.157.135.157:59747', 'Over', 'Chat', 'GM', 'Message: oO');
INSERT INTO `log` VALUES ('221', '2010-12-07 17:51:45', '84.157.135.157:59747', 'Over', 'GM', 'Custom_Command', 'Command: jetzt kann ich es aufheben');
INSERT INTO `log` VALUES ('222', '2010-12-07 17:51:45', '84.157.135.157:59747', 'Over', 'Chat', 'GM', 'Message: jetzt kann ich es aufheben');
INSERT INTO `log` VALUES ('223', '2010-12-07 17:51:56', '88.68.33.78:2255', 'dongdong', 'GM', 'Custom_Command', 'Command: lol');
INSERT INTO `log` VALUES ('224', '2010-12-07 17:51:56', '88.68.33.78:2255', 'dongdong', 'Chat', 'GM', 'Message: lol');
INSERT INTO `log` VALUES ('225', '2010-12-07 17:52:04', '84.157.135.157:59747', 'Over', 'GM', 'Custom_Command', 'Command: aber erst nach einem restart');
INSERT INTO `log` VALUES ('226', '2010-12-07 17:52:04', '84.157.135.157:59747', 'Over', 'Chat', 'GM', 'Message: aber erst nach einem restart');
INSERT INTO `log` VALUES ('227', '2010-12-07 17:52:17', '84.157.135.157:59747', 'Over', 'GM', 'Custom_Command', 'Command: mhm');
INSERT INTO `log` VALUES ('228', '2010-12-07 17:52:17', '84.157.135.157:59747', 'Over', 'Chat', 'GM', 'Message: mhm');
INSERT INTO `log` VALUES ('229', '2010-12-07 17:52:21', '84.157.135.157:59747', 'Over', 'GM', 'Custom_Command', 'Command: oder auch nciht');
INSERT INTO `log` VALUES ('230', '2010-12-07 17:52:21', '84.157.135.157:59747', 'Over', 'Chat', 'GM', 'Message: oder auch nciht');
INSERT INTO `log` VALUES ('231', '2010-12-07 17:52:31', '88.68.33.78:2255', 'dongdong', 'GM', 'Custom_Command', 'Command: das kann ich nicht aufheben ');
INSERT INTO `log` VALUES ('232', '2010-12-07 17:52:31', '88.68.33.78:2255', 'dongdong', 'Chat', 'GM', 'Message: das kann ich nicht aufheben ');
INSERT INTO `log` VALUES ('233', '2010-12-07 17:52:55', '84.157.135.157:59747', 'Over', 'GM', 'Custom_Command', 'Command: was mich wundert');
INSERT INTO `log` VALUES ('234', '2010-12-07 17:52:55', '84.157.135.157:59747', 'Over', 'Chat', 'GM', 'Message: was mich wundert');
INSERT INTO `log` VALUES ('235', '2010-12-07 17:53:01', '84.157.135.157:59747', 'Over', 'GM', 'Custom_Command', 'Command: der autospawn fehlt');
INSERT INTO `log` VALUES ('236', '2010-12-07 17:53:01', '84.157.135.157:59747', 'Over', 'Chat', 'GM', 'Message: der autospawn fehlt');
INSERT INTO `log` VALUES ('237', '2010-12-07 17:58:40', '88.68.33.78:2277', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('238', '2010-12-07 17:58:55', '88.68.33.78:2277', '[UNKNWON]', 'Login', 'Sucess', 'Name: dongdong, Server: Remote');
INSERT INTO `log` VALUES ('239', '2010-12-07 18:01:11', '84.157.135.157:59831', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('240', '2010-12-07 18:02:27', '84.157.135.157:59831', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('241', '2010-12-07 18:03:08', '88.68.33.78:2289', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('242', '2010-12-07 18:03:26', '88.68.33.78:2289', '[UNKNWON]', 'Login', 'Sucess', 'Name: dongdong, Server: Remote');
INSERT INTO `log` VALUES ('243', '2010-12-07 18:04:29', '88.68.33.78:2292', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('244', '2010-12-07 18:05:03', '88.68.33.78:2292', '[UNKNWON]', 'Login', 'Sucess', 'Name: dongdong, Server: Remote');
INSERT INTO `log` VALUES ('245', '2010-12-08 22:34:05', '84.157.134.55:63593', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('246', '2010-12-08 22:34:12', '84.157.134.55:63593', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('247', '2010-12-08 22:44:57', '84.157.134.55:64216', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('248', '2010-12-08 22:45:03', '84.157.134.55:64216', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Remote');
INSERT INTO `log` VALUES ('249', '2010-12-08 22:46:41', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('250', '2010-12-08 22:46:41', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('251', '2010-12-08 22:46:42', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('252', '2010-12-08 22:46:42', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('253', '2010-12-08 22:46:43', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('254', '2010-12-08 22:46:43', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('255', '2010-12-08 22:46:43', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('256', '2010-12-08 22:46:43', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('257', '2010-12-08 22:46:44', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('258', '2010-12-08 22:46:44', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('259', '2010-12-08 22:46:45', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('260', '2010-12-08 22:46:45', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('261', '2010-12-08 22:46:45', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('262', '2010-12-08 22:46:45', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('263', '2010-12-08 22:46:45', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('264', '2010-12-08 22:46:46', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('265', '2010-12-08 22:46:46', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('266', '2010-12-08 22:46:46', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('267', '2010-12-08 22:46:47', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('268', '2010-12-08 22:46:47', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('269', '2010-12-08 22:46:47', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('270', '2010-12-08 22:46:47', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('271', '2010-12-08 22:46:48', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('272', '2010-12-08 22:46:48', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('273', '2010-12-08 22:46:48', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('274', '2010-12-08 22:46:48', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('275', '2010-12-08 22:46:49', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('276', '2010-12-08 22:46:49', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('277', '2010-12-08 22:46:49', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('278', '2010-12-08 22:46:49', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('279', '2010-12-08 22:46:50', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('280', '2010-12-08 22:46:50', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('281', '2010-12-08 22:46:50', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('282', '2010-12-08 22:46:50', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('283', '2010-12-08 22:46:51', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('284', '2010-12-08 22:46:51', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('285', '2010-12-08 22:46:51', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('286', '2010-12-08 22:46:51', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('287', '2010-12-08 22:46:52', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('288', '2010-12-08 22:46:52', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('289', '2010-12-08 22:46:52', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('290', '2010-12-08 22:46:52', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('291', '2010-12-08 22:46:53', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('292', '2010-12-08 22:46:53', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('293', '2010-12-08 22:46:53', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('294', '2010-12-08 22:46:53', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('295', '2010-12-08 22:46:54', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('296', '2010-12-08 22:46:54', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('297', '2010-12-08 22:46:54', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('298', '2010-12-08 22:46:54', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('299', '2010-12-08 22:46:55', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('300', '2010-12-08 22:46:55', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('301', '2010-12-08 22:46:55', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('302', '2010-12-08 22:46:55', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('303', '2010-12-08 22:46:56', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('304', '2010-12-08 22:46:56', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('305', '2010-12-08 22:46:56', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('306', '2010-12-08 22:46:56', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('307', '2010-12-08 22:46:57', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('308', '2010-12-08 22:46:58', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('309', '2010-12-08 22:46:58', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('310', '2010-12-08 22:46:59', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('311', '2010-12-08 22:46:59', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('312', '2010-12-08 22:46:59', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('313', '2010-12-08 22:46:59', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('314', '2010-12-08 22:46:59', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('315', '2010-12-08 22:47:00', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('316', '2010-12-08 22:47:00', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('317', '2010-12-08 22:47:00', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('318', '2010-12-08 22:47:00', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('319', '2010-12-08 22:47:01', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('320', '2010-12-08 22:47:01', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('321', '2010-12-08 22:47:01', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('322', '2010-12-08 22:47:01', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('323', '2010-12-08 22:47:02', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('324', '2010-12-08 22:47:02', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('325', '2010-12-08 22:47:02', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('326', '2010-12-08 22:47:03', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('327', '2010-12-08 22:47:03', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('328', '2010-12-08 22:47:03', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('329', '2010-12-08 22:47:03', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('330', '2010-12-08 22:47:03', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('331', '2010-12-08 22:47:04', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('332', '2010-12-08 22:47:05', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('333', '2010-12-08 22:47:06', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('334', '2010-12-08 22:47:06', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('335', '2010-12-08 22:47:06', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('336', '2010-12-08 22:47:06', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('337', '2010-12-08 22:47:07', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('338', '2010-12-08 22:47:07', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('339', '2010-12-08 22:47:07', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('340', '2010-12-08 22:47:07', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('341', '2010-12-08 22:47:08', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('342', '2010-12-08 22:47:08', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('343', '2010-12-08 22:47:08', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('344', '2010-12-08 22:47:08', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('345', '2010-12-08 22:47:09', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 1');
INSERT INTO `log` VALUES ('346', '2010-12-08 22:47:11', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('347', '2010-12-08 22:47:11', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('348', '2010-12-08 22:47:11', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('349', '2010-12-08 22:47:12', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('350', '2010-12-08 22:47:12', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('351', '2010-12-08 22:47:12', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('352', '2010-12-08 22:47:13', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('353', '2010-12-08 22:47:13', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('354', '2010-12-08 22:47:13', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('355', '2010-12-08 22:47:13', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('356', '2010-12-08 22:47:15', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('357', '2010-12-08 22:47:15', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('358', '2010-12-08 22:47:15', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('359', '2010-12-08 22:47:16', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('360', '2010-12-08 22:47:16', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('361', '2010-12-08 22:47:16', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('362', '2010-12-08 22:47:16', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('363', '2010-12-08 22:47:17', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('364', '2010-12-08 22:47:17', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('365', '2010-12-08 22:47:17', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('366', '2010-12-08 22:47:17', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('367', '2010-12-08 22:47:18', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('368', '2010-12-08 22:47:18', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('369', '2010-12-08 22:47:18', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('370', '2010-12-08 22:47:18', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('371', '2010-12-08 22:47:19', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('372', '2010-12-08 22:47:19', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('373', '2010-12-08 22:47:19', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('374', '2010-12-08 22:47:19', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('375', '2010-12-08 22:47:20', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('376', '2010-12-08 22:48:09', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('377', '2010-12-08 22:48:09', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('378', '2010-12-08 22:48:10', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('379', '2010-12-08 22:48:10', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('380', '2010-12-08 22:48:10', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('381', '2010-12-08 22:48:11', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('382', '2010-12-08 22:48:11', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('383', '2010-12-08 22:48:11', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('384', '2010-12-08 22:48:11', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('385', '2010-12-08 22:50:50', '84.157.134.55:64217', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 1933, Monster_Name: MOB_CH_MANGNYANG Type: 0');
INSERT INTO `log` VALUES ('386', '2010-12-10 20:40:36', '127.0.0.1:53366', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('387', '2010-12-10 20:40:56', '127.0.0.1:53366', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('388', '2010-12-10 20:44:09', '127.0.0.1:53374', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('389', '2010-12-10 20:44:20', '127.0.0.1:53374', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('390', '2010-12-10 20:46:11', '127.0.0.1:53375', 'Over', 'Chat', 'GM', 'Message: \\level 1');
INSERT INTO `log` VALUES ('391', '2010-12-10 20:48:07', '127.0.0.1:53375', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 2002, Monster_Name: MOB_KK_ISYUTARU Type: 0');
INSERT INTO `log` VALUES ('392', '2010-12-10 21:01:09', '127.0.0.1:53375', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 2002, Monster_Name: MOB_KK_ISYUTARU Type: 0');
INSERT INTO `log` VALUES ('393', '2010-12-10 21:05:45', '127.0.0.1:53375', 'Over', 'Chat', 'GM', 'Message: \\count');
INSERT INTO `log` VALUES ('394', '2010-12-10 21:06:46', '127.0.0.1:53375', 'Over', 'Chat', 'GM', 'Message: \\count');
INSERT INTO `log` VALUES ('395', '2010-12-10 21:15:49', '127.0.0.1:53900', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('396', '2010-12-10 21:15:57', '127.0.0.1:53900', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('397', '2010-12-10 21:17:20', '127.0.0.1:53901', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 3875, Monster_Name: MOB_RM_TAHOMET Type: 0');
INSERT INTO `log` VALUES ('398', '2010-12-10 21:21:04', '127.0.0.1:53901', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 3875, Monster_Name: MOB_RM_TAHOMET Type: 0');
INSERT INTO `log` VALUES ('399', '2010-12-10 21:52:56', '127.0.0.1:54465', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('400', '2010-12-10 21:53:04', '127.0.0.1:54465', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('401', '2010-12-10 21:54:07', '127.0.0.1:54647', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('402', '2010-12-10 21:54:54', '127.0.0.1:54711', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('403', '2010-12-10 21:56:50', '127.0.0.1:54711', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('404', '2010-12-10 21:58:20', '127.0.0.1:54827', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 3810, Monster_Name: MOB_TK_BONELORD Type: 3');
INSERT INTO `log` VALUES ('405', '2010-12-10 22:03:27', '127.0.0.1:54827', 'Over', 'GM', 'Monster_Spawn', 'PK2ID: 3810, Monster_Name:  Type: 0');
INSERT INTO `log` VALUES ('406', '2010-12-10 22:19:01', '127.0.0.1:55069', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('407', '2010-12-10 22:19:13', '127.0.0.1:55069', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('408', '2010-12-10 22:36:49', '127.0.0.1:55347', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('409', '2010-12-10 22:38:44', '127.0.0.1:55347', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('410', '2010-12-10 22:39:51', '127.0.0.1:55503', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('411', '2010-12-10 22:40:07', '127.0.0.1:55504', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('412', '2010-12-10 22:40:23', '127.0.0.1:55504', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('413', '2010-12-10 22:54:58', '127.0.0.1:55539', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('414', '2010-12-10 22:55:12', '127.0.0.1:55540', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('415', '2010-12-10 22:56:41', '127.0.0.1:55540', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('416', '2010-12-10 22:41:08', '127.0.0.1:55505', 'Over', 'Chat', 'GM', 'Message: \\level 140');
INSERT INTO `log` VALUES ('417', '2010-12-10 23:01:36', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 280 ');
INSERT INTO `log` VALUES ('418', '2010-12-10 23:02:12', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 40');
INSERT INTO `log` VALUES ('419', '2010-12-10 23:02:25', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 80');
INSERT INTO `log` VALUES ('420', '2010-12-10 23:02:45', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 300');
INSERT INTO `log` VALUES ('421', '2010-12-10 23:02:54', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 280');
INSERT INTO `log` VALUES ('422', '2010-12-10 23:03:05', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 320');
INSERT INTO `log` VALUES ('423', '2010-12-10 23:03:17', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 320');
INSERT INTO `log` VALUES ('424', '2010-12-10 23:03:36', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 260');
INSERT INTO `log` VALUES ('425', '2010-12-10 23:04:00', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\tun 45');
INSERT INTO `log` VALUES ('426', '2010-12-10 23:04:44', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 130');
INSERT INTO `log` VALUES ('427', '2010-12-10 23:05:02', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 90');
INSERT INTO `log` VALUES ('428', '2010-12-10 23:05:19', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 45');
INSERT INTO `log` VALUES ('429', '2010-12-10 23:05:44', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 85');
INSERT INTO `log` VALUES ('430', '2010-12-10 23:05:58', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 90');
INSERT INTO `log` VALUES ('431', '2010-12-10 23:06:23', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 0');
INSERT INTO `log` VALUES ('432', '2010-12-10 23:06:40', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 180');
INSERT INTO `log` VALUES ('433', '2010-12-10 23:07:01', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 180');
INSERT INTO `log` VALUES ('434', '2010-12-10 23:07:15', '127.0.0.1:55660', 'Over', 'Chat', 'GM', 'Message: \\turn 180');
INSERT INTO `log` VALUES ('435', '2010-12-10 23:13:16', '127.0.0.1:55832', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('436', '2010-12-10 23:13:31', '127.0.0.1:55832', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('437', '2010-12-10 23:32:59', '127.0.0.1:55850', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('438', '2010-12-10 23:33:09', '127.0.0.1:55850', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('439', '2010-12-10 23:39:33', '127.0.0.1:55864', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('440', '2010-12-10 23:39:41', '127.0.0.1:55864', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('441', '2010-12-10 23:40:14', '127.0.0.1:55866', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('442', '2010-12-10 23:40:52', '127.0.0.1:55866', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('443', '2010-12-10 23:43:43', '127.0.0.1:55875', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('444', '2010-12-10 23:43:50', '127.0.0.1:55875', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `log` VALUES ('445', '2010-12-10 23:46:48', '127.0.0.1:55878', '[UNKNWON]', 'Client_Connect', '(None)', 'Locale: 40, Name: SR_Client, Version: 18');
INSERT INTO `log` VALUES ('446', '2010-12-10 23:54:42', '127.0.0.1:55878', '[UNKNWON]', 'Login', 'Sucess', 'Name: i, Server: Local');
INSERT INTO `masteries` VALUES ('269', '1', '257', '0');
INSERT INTO `masteries` VALUES ('270', '1', '258', '0');
INSERT INTO `masteries` VALUES ('271', '1', '259', '0');
INSERT INTO `masteries` VALUES ('272', '1', '273', '0');
INSERT INTO `masteries` VALUES ('273', '1', '274', '0');
INSERT INTO `masteries` VALUES ('274', '1', '275', '0');
INSERT INTO `masteries` VALUES ('275', '1', '276', '0');
INSERT INTO `masteries` VALUES ('276', '8780', '257', '0');
INSERT INTO `masteries` VALUES ('277', '8780', '258', '0');
INSERT INTO `masteries` VALUES ('278', '8780', '259', '0');
INSERT INTO `masteries` VALUES ('279', '8780', '273', '0');
INSERT INTO `masteries` VALUES ('280', '8780', '274', '0');
INSERT INTO `masteries` VALUES ('281', '8780', '275', '0');
INSERT INTO `masteries` VALUES ('282', '8780', '276', '0');
INSERT INTO `masteries` VALUES ('283', '8781', '513', '0');
INSERT INTO `masteries` VALUES ('284', '8781', '514', '0');
INSERT INTO `masteries` VALUES ('285', '8781', '515', '0');
INSERT INTO `masteries` VALUES ('286', '8781', '516', '0');
INSERT INTO `masteries` VALUES ('287', '8781', '517', '0');
INSERT INTO `masteries` VALUES ('288', '8781', '518', '0');
INSERT INTO `masteries` VALUES ('289', '8782', '257', '0');
INSERT INTO `masteries` VALUES ('290', '8782', '258', '0');
INSERT INTO `masteries` VALUES ('291', '8782', '259', '0');
INSERT INTO `masteries` VALUES ('292', '8782', '273', '0');
INSERT INTO `masteries` VALUES ('293', '8782', '274', '0');
INSERT INTO `masteries` VALUES ('294', '8782', '275', '0');
INSERT INTO `masteries` VALUES ('295', '8782', '276', '0');
INSERT INTO `masteries` VALUES ('296', '8783', '257', '0');
INSERT INTO `masteries` VALUES ('297', '8783', '258', '0');
INSERT INTO `masteries` VALUES ('298', '8783', '259', '0');
INSERT INTO `masteries` VALUES ('299', '8783', '273', '0');
INSERT INTO `masteries` VALUES ('300', '8783', '274', '0');
INSERT INTO `masteries` VALUES ('301', '8783', '275', '0');
INSERT INTO `masteries` VALUES ('302', '8783', '276', '0');
INSERT INTO `masteries` VALUES ('303', '8785', '257', '0');
INSERT INTO `masteries` VALUES ('304', '8785', '258', '0');
INSERT INTO `masteries` VALUES ('305', '8785', '259', '0');
INSERT INTO `masteries` VALUES ('306', '8785', '273', '0');
INSERT INTO `masteries` VALUES ('307', '8785', '274', '0');
INSERT INTO `masteries` VALUES ('308', '8785', '275', '0');
INSERT INTO `masteries` VALUES ('309', '8785', '276', '0');
INSERT INTO `masteries` VALUES ('310', '8840', '257', '0');
INSERT INTO `masteries` VALUES ('311', '8840', '258', '0');
INSERT INTO `masteries` VALUES ('312', '8840', '259', '0');
INSERT INTO `masteries` VALUES ('313', '8840', '273', '0');
INSERT INTO `masteries` VALUES ('314', '8840', '274', '0');
INSERT INTO `masteries` VALUES ('315', '8840', '275', '0');
INSERT INTO `masteries` VALUES ('316', '8840', '276', '0');
INSERT INTO `news` VALUES ('1', 'Opening', '<center><font color=red><b>Welcome to the Visual Silkroad Project</b></font></center>', '3000-01-01 00:00:00');
INSERT INTO `positions` VALUES ('1', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `positions` VALUES ('8780', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `positions` VALUES ('8781', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `positions` VALUES ('8782', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `positions` VALUES ('8783', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `positions` VALUES ('8785', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `positions` VALUES ('8840', '168', '97', '980', '1330', '65504', '168', '97', '1048', '1713', '-4', '168', '97', '980', '1330', '65504');
INSERT INTO `servers` VALUES ('3', 'Local', '0', '500', '1', '127.0.0.1', '15780');
INSERT INTO `servers` VALUES ('4', 'Remote', '0', '500', '1', '78.111.78.27', '15780');
INSERT INTO `users` VALUES ('3', 'i', 'i', '0', '0', 'You got banned for 10 Minutes because of 5 failed Logins.', '2010-10-01 16:01:38', '6001');
INSERT INTO `users` VALUES ('9', 'test', 'test', '0', '0', '...', '3000-01-01 00:00:00', '431');
INSERT INTO `users` VALUES ('10', 'new', 'new', '0', '0', '...', '3000-01-01 00:00:00', '51');
INSERT INTO `users` VALUES ('11', 'new ', 'new', '0', '0', '...', '3000-01-01 00:00:00', '500');
INSERT INTO `users` VALUES ('12', 'eckonet', '1234567891', '0', '0', '...', '3000-01-01 00:00:00', '500');
INSERT INTO `users` VALUES ('13', 'dongdong', '123polizei', '3', '0', '...', '3000-01-01 00:00:00', '831');
