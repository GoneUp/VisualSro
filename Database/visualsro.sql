/*
MySQL Data Transfer
Source Host: localhost
Source Database: visualsro
Target Host: localhost
Target Database: visualsro
Date: 16.10.2010 16:12:14
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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=934 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=237 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `news` VALUES ('1', 'Opening', '<center><font color=red><b>Welcome to the Visual Silkroad Project</b></font></center>', '25', '4');
INSERT INTO `servers` VALUES ('3', 'Local', '0', '500', '1', '127.0.0.1', '15780');
INSERT INTO `servers` VALUES ('4', 'Remote', '0', '500', '1', '84.157.143.106', '15780');
INSERT INTO `users` VALUES ('2', 'test', 'test', '1', '0', '...', '3000-01-02');
INSERT INTO `users` VALUES ('3', 'i', 'i', '1', '0', 'You got banned by: __________12', '3000-01-01');
INSERT INTO `users` VALUES ('5', 'maex', 'maex', '0', '0', 'So halt^^', '3000-01-01');
INSERT INTO `users` VALUES ('6', 'register', 'test', '0', '0', '...', '3000-01-01');
INSERT INTO `users` VALUES ('7', 'newacc', 'test', '0', '0', '...', '3000-01-01');
