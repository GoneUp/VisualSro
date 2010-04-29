/*
MySQL Data Transfer
Source Host: localhost
Source Database: visualsro
Target Host: localhost
Target Database: visualsro
Date: 29.04.2010 14:31:19
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
  `volume` int(10) unsigned NOT NULL DEFAULT '22',
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for masteries
-- ----------------------------
DROP TABLE IF EXISTS `masteries`;
CREATE TABLE `masteries` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `mastery` int(10) unsigned NOT NULL,
  `slevel` int(10) unsigned NOT NULL DEFAULT '0',
  `owner` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

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
  `banned` int(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `characters` VALUES ('8', '2', 'GoneUp', '1907', '34', '1', '0', '20', '30', '0', '200', '300', '0', '0', '0', '0', '1', '168', '98', '978', '1097', '40', '200', '300', '5', '10', '5', '10', '9', '7', '3', '5', '16', '50', '100', '0', '254', '45');
INSERT INTO `characters` VALUES ('9', '2', 'manneke', '1907', '22', '1', '0', '20', '30', '0', '200', '300', '0', '0', '0', '0', '0', '168', '98', '978', '1097', '40', '200', '300', '5', '10', '5', '10', '9', '7', '3', '5', '16', '50', '100', '0', '255', '45');
INSERT INTO `characters` VALUES ('10', '2', 'Windrius', '1907', '22', '1', '0', '20', '30', '0', '200', '300', '0', '0', '0', '0', '0', '168', '98', '978', '1097', '40', '200', '300', '5', '10', '5', '10', '9', '7', '3', '5', '16', '50', '100', '0', '255', '45');
INSERT INTO `items` VALUES ('1', '133', '8', '20', '6', '0', '1', '30');
INSERT INTO `items` VALUES ('2', '277', '8', '15', '7', '0', '1', '30');
INSERT INTO `news` VALUES ('1', 'Opening', '<center><font color=red><b>Welcome to the Visual Silkroad Project</b></font></center>', '25', '4');
INSERT INTO `servers` VALUES ('3', 'VisualSro', '0', '500', '1', '127.0.0.1', '15780');
INSERT INTO `servers` VALUES ('8', 'Test2', '250', '500', '1', '127.0.0.1', '15780');
INSERT INTO `users` VALUES ('2', 'test', 'test', '0', '0');
INSERT INTO `users` VALUES ('3', 'test2', 'test', '2', '1');
