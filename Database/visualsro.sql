/*
MySQL Data Transfer
Source Host: localhost
Source Database: visualsro
Target Host: localhost
Target Database: visualsro
Date: 14.08.2010 20:52:43
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for characters
-- ----------------------------
DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
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
  `deletion_time` int(10) unsigned NOT NULL DEFAULT '0',
  `gold` int(10) unsigned NOT NULL DEFAULT '0',
  `sp` int(10) unsigned NOT NULL DEFAULT '0',
  `gm` tinyint(1) NOT NULL DEFAULT '0',
  `xsect` int(10) unsigned NOT NULL DEFAULT '168' COMMENT 'Default loc Jangan',
  `ysect` int(10) unsigned NOT NULL DEFAULT '98',
  `xpos` int(10) unsigned NOT NULL DEFAULT '978',
  `ypos` int(10) unsigned NOT NULL DEFAULT '1097',
  `zpos` int(10) unsigned NOT NULL DEFAULT '40',
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
  `maxitemslots` int(1) NOT NULL DEFAULT '45',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=38 DEFAULT CHARSET=latin1;

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
  `type` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'CH = 0, ETC = 1',
  `quantity` int(10) unsigned NOT NULL DEFAULT '1',
  `durability` int(10) unsigned NOT NULL DEFAULT '30',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;

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
  `bantime` varchar(255) NOT NULL DEFAULT '3000-01-01',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `characters` VALUES ('32', '3', 'CH', '1907', '34', '1', '0', '20', '30', '0', '200', '300', '0', '0', '0', '0', '0', '168', '98', '978', '1097', '40', '200', '300', '5', '10', '5', '10', '9', '7', '3', '5', '16', '50', '100', '0', '255', '45');
INSERT INTO `characters` VALUES ('33', '3', 'CH1', '1907', '34', '1', '0', '20', '30', '0', '200', '300', '0', '0', '0', '0', '0', '168', '98', '978', '1097', '40', '200', '300', '5', '10', '5', '10', '9', '7', '3', '5', '16', '50', '100', '0', '255', '45');
INSERT INTO `characters` VALUES ('34', '3', 'EU', '14717', '34', '1', '0', '20', '30', '0', '200', '300', '0', '0', '0', '0', '0', '168', '98', '978', '1097', '40', '200', '300', '5', '10', '5', '10', '9', '7', '3', '5', '16', '50', '100', '0', '255', '45');
INSERT INTO `characters` VALUES ('35', '3', 'EU_Shield', '14717', '34', '1', '0', '20', '30', '0', '200', '300', '0', '0', '0', '0', '0', '168', '98', '978', '1097', '40', '200', '300', '5', '10', '5', '10', '9', '7', '3', '5', '16', '50', '100', '0', '255', '45');
INSERT INTO `characters` VALUES ('36', '2', 'EU9', '14717', '34', '1', '0', '20', '30', '0', '200', '300', '0', '0', '0', '0', '0', '168', '98', '978', '1097', '40', '200', '300', '5', '10', '5', '10', '9', '7', '3', '5', '16', '50', '100', '0', '255', '45');
INSERT INTO `characters` VALUES ('37', '2', 'EU8', '14717', '34', '1', '0', '20', '30', '0', '200', '300', '0', '0', '0', '0', '0', '168', '98', '978', '1097', '40', '200', '300', '5', '10', '5', '10', '9', '7', '3', '5', '16', '50', '100', '0', '255', '45');
INSERT INTO `items` VALUES ('11', '3637', '8', '2', '1', '0', '1', '30');
INSERT INTO `items` VALUES ('12', '3638', '8', '2', '4', '0', '1', '30');
INSERT INTO `items` VALUES ('13', '3639', '8', '2', '5', '0', '1', '30');
INSERT INTO `items` VALUES ('14', '3632', '8', '1', '6', '0', '1', '30');
INSERT INTO `items` VALUES ('15', '251', '8', '2', '7', '0', '1', '30');
INSERT INTO `items` VALUES ('16', '3637', '8', '2', '1', '0', '1', '30');
INSERT INTO `items` VALUES ('17', '3638', '8', '2', '4', '0', '1', '30');
INSERT INTO `items` VALUES ('18', '3639', '8', '2', '5', '0', '1', '30');
INSERT INTO `items` VALUES ('19', '3632', '8', '1', '6', '0', '1', '30');
INSERT INTO `items` VALUES ('20', '251', '8', '2', '7', '0', '1', '30');
INSERT INTO `items` VALUES ('21', '3643', '2', '2', '1', '0', '1', '30');
INSERT INTO `items` VALUES ('22', '3644', '2', '2', '4', '0', '1', '30');
INSERT INTO `items` VALUES ('23', '3645', '2', '2', '5', '0', '1', '30');
INSERT INTO `items` VALUES ('24', '3636', '2', '1', '6', '0', '1', '30');
INSERT INTO `items` VALUES ('25', '62', '2', '0', '7', '0', '1', '30');
INSERT INTO `items` VALUES ('26', '11465', '3', '1', '1', '0', '1', '30');
INSERT INTO `items` VALUES ('27', '11466', '3', '2', '4', '0', '1', '30');
INSERT INTO `items` VALUES ('28', '11467', '3', '0', '5', '0', '1', '30');
INSERT INTO `items` VALUES ('29', '10736', '3', '4', '6', '0', '1', '30');
INSERT INTO `items` VALUES ('30', '11462', '4', '2', '1', '0', '1', '30');
INSERT INTO `items` VALUES ('31', '11463', '4', '2', '4', '0', '1', '30');
INSERT INTO `items` VALUES ('32', '11464', '4', '0', '5', '0', '1', '30');
INSERT INTO `items` VALUES ('33', '10730', '4', '2', '6', '0', '1', '30');
INSERT INTO `items` VALUES ('34', '10738', '4', '4', '7', '0', '1', '30');
INSERT INTO `items` VALUES ('35', '11465', '5', '2', '1', '0', '1', '30');
INSERT INTO `items` VALUES ('36', '11466', '5', '1', '4', '0', '1', '30');
INSERT INTO `items` VALUES ('37', '11467', '5', '3', '5', '0', '1', '30');
INSERT INTO `items` VALUES ('38', '10734', '5', '4', '6', '0', '1', '30');
INSERT INTO `items` VALUES ('39', '10738', '5', '0', '7', '0', '1', '30');
INSERT INTO `items` VALUES ('40', '11465', '6', '3', '1', '0', '1', '30');
INSERT INTO `items` VALUES ('41', '11466', '6', '1', '4', '0', '1', '30');
INSERT INTO `items` VALUES ('42', '11467', '6', '2', '5', '0', '1', '30');
INSERT INTO `items` VALUES ('43', '10737', '6', '4', '6', '0', '1', '30');
INSERT INTO `items` VALUES ('44', '10738', '6', '0', '7', '0', '1', '30');
INSERT INTO `masteries` VALUES ('7', '0', '0', '1');
INSERT INTO `masteries` VALUES ('8', '2', '257', '1');
INSERT INTO `masteries` VALUES ('9', '2', '258', '1');
INSERT INTO `masteries` VALUES ('10', '2', '259', '1');
INSERT INTO `masteries` VALUES ('11', '2', '273', '1');
INSERT INTO `masteries` VALUES ('12', '2', '274', '1');
INSERT INTO `masteries` VALUES ('13', '2', '275', '1');
INSERT INTO `masteries` VALUES ('14', '2', '276', '1');
INSERT INTO `masteries` VALUES ('15', '3', '513', '1');
INSERT INTO `masteries` VALUES ('16', '3', '514', '1');
INSERT INTO `masteries` VALUES ('17', '3', '515', '1');
INSERT INTO `masteries` VALUES ('18', '3', '516', '1');
INSERT INTO `masteries` VALUES ('19', '3', '517', '1');
INSERT INTO `masteries` VALUES ('20', '3', '518', '1');
INSERT INTO `masteries` VALUES ('21', '4', '513', '1');
INSERT INTO `masteries` VALUES ('22', '4', '514', '1');
INSERT INTO `masteries` VALUES ('23', '4', '515', '1');
INSERT INTO `masteries` VALUES ('24', '4', '516', '1');
INSERT INTO `masteries` VALUES ('25', '4', '517', '1');
INSERT INTO `masteries` VALUES ('26', '4', '518', '1');
INSERT INTO `masteries` VALUES ('27', '5', '513', '1');
INSERT INTO `masteries` VALUES ('28', '5', '514', '1');
INSERT INTO `masteries` VALUES ('29', '5', '515', '1');
INSERT INTO `masteries` VALUES ('30', '5', '516', '1');
INSERT INTO `masteries` VALUES ('31', '5', '517', '1');
INSERT INTO `masteries` VALUES ('32', '5', '518', '1');
INSERT INTO `masteries` VALUES ('33', '6', '513', '1');
INSERT INTO `masteries` VALUES ('34', '6', '514', '1');
INSERT INTO `masteries` VALUES ('35', '6', '515', '1');
INSERT INTO `masteries` VALUES ('36', '6', '516', '1');
INSERT INTO `masteries` VALUES ('37', '6', '517', '1');
INSERT INTO `masteries` VALUES ('38', '6', '518', '1');
INSERT INTO `news` VALUES ('1', 'Opening', '<center><font color=red><b>Welcome to the Visual Silkroad Project</b></font></center>', '25', '4');
INSERT INTO `servers` VALUES ('3', 'VisualSro', '0', '500', '1', '127.0.0.1', '15780');
INSERT INTO `servers` VALUES ('8', 'Test2', '250', '500', '1', '127.0.0.1', '15780');
INSERT INTO `users` VALUES ('2', 'test', 'test', '2', '0', '...', '3000-01-02');
INSERT INTO `users` VALUES ('3', 'i', 'i', '2', '0', '...', '3000-01-02');
