/*
MySQL Data Transfer
Source Host: localhost
Source Database: visualsro
Target Host: localhost
Target Database: visualsro
Date: 30.10.2010 13:56:01
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
) ENGINE=InnoDB AUTO_INCREMENT=206 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=1159 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=269 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `characters` VALUES ('1', '3', 'Over', '1907', '34', '100', '0', '20', '30', '0', '1421', '2131', '0', '1000-01-01 00:00:00', '1000000', '1000000', '-1', '168', '98', '952', '256', '1', '1421', '300', '6', '109', '10', '10', '6', '9', '110', '110', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `characters` VALUES ('2', '3', 'Never', '1907', '34', '100', '0', '20', '20', '0', '1421', '1421', '0', '1000-01-01 00:00:00', '1000000', '999992', '-1', '107', '107', '465', '11', '180', '1421', '300', '240', '275', '343', '343', '8', '6', '110', '110', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `characters` VALUES ('5', '2', 'Ra', '14717', '34', '100', '0', '20', '20', '0', '1421', '1421', '0', '1000-01-01 00:00:00', '1000000', '1000000', '-1', '168', '98', '980', '330', '20', '200', '300', '6', '109', '7', '7', '3', '6', '110', '110', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `characters` VALUES ('23', '9', '_4_3_2_1_', '14720', '68', '100', '0', '20', '20', '0', '1421', '1421', '0', '1000-01-01 00:00:00', '1000000', '1000000', '-1', '168', '96', '1051', '1380', '-9', '200', '300', '6', '1472', '2147', '2147', '53', '6', '110', '110', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `hotkeys` VALUES ('2', '2', '0', '0', '0');
INSERT INTO `hotkeys` VALUES ('3', '2', '1', '73', '3');
INSERT INTO `hotkeys` VALUES ('4', '2', '2', '0', '0');
INSERT INTO `hotkeys` VALUES ('5', '2', '3', '0', '0');
INSERT INTO `hotkeys` VALUES ('6', '2', '4', '0', '0');
INSERT INTO `hotkeys` VALUES ('7', '2', '5', '0', '0');
INSERT INTO `hotkeys` VALUES ('8', '2', '6', '0', '0');
INSERT INTO `hotkeys` VALUES ('9', '2', '7', '0', '0');
INSERT INTO `hotkeys` VALUES ('10', '2', '8', '0', '0');
INSERT INTO `hotkeys` VALUES ('11', '2', '9', '0', '0');
INSERT INTO `hotkeys` VALUES ('12', '2', '10', '0', '0');
INSERT INTO `hotkeys` VALUES ('13', '2', '11', '0', '0');
INSERT INTO `hotkeys` VALUES ('14', '2', '12', '0', '0');
INSERT INTO `hotkeys` VALUES ('15', '2', '13', '0', '0');
INSERT INTO `hotkeys` VALUES ('16', '2', '14', '0', '0');
INSERT INTO `hotkeys` VALUES ('17', '2', '15', '0', '0');
INSERT INTO `hotkeys` VALUES ('18', '2', '16', '0', '0');
INSERT INTO `hotkeys` VALUES ('19', '2', '17', '0', '0');
INSERT INTO `hotkeys` VALUES ('20', '2', '18', '0', '0');
INSERT INTO `hotkeys` VALUES ('21', '2', '19', '0', '0');
INSERT INTO `hotkeys` VALUES ('22', '2', '20', '0', '0');
INSERT INTO `hotkeys` VALUES ('23', '2', '21', '0', '0');
INSERT INTO `hotkeys` VALUES ('24', '2', '22', '0', '0');
INSERT INTO `hotkeys` VALUES ('25', '2', '23', '0', '0');
INSERT INTO `hotkeys` VALUES ('26', '2', '24', '0', '0');
INSERT INTO `hotkeys` VALUES ('27', '2', '25', '0', '0');
INSERT INTO `hotkeys` VALUES ('28', '2', '26', '0', '0');
INSERT INTO `hotkeys` VALUES ('29', '2', '27', '0', '0');
INSERT INTO `hotkeys` VALUES ('30', '2', '28', '0', '0');
INSERT INTO `hotkeys` VALUES ('31', '2', '29', '0', '0');
INSERT INTO `hotkeys` VALUES ('32', '2', '30', '0', '0');
INSERT INTO `hotkeys` VALUES ('33', '2', '31', '0', '0');
INSERT INTO `hotkeys` VALUES ('34', '2', '32', '0', '0');
INSERT INTO `hotkeys` VALUES ('35', '2', '33', '0', '0');
INSERT INTO `hotkeys` VALUES ('36', '2', '34', '0', '0');
INSERT INTO `hotkeys` VALUES ('37', '2', '35', '0', '0');
INSERT INTO `hotkeys` VALUES ('38', '2', '36', '0', '0');
INSERT INTO `hotkeys` VALUES ('39', '2', '37', '0', '0');
INSERT INTO `hotkeys` VALUES ('40', '2', '38', '0', '0');
INSERT INTO `hotkeys` VALUES ('41', '2', '39', '0', '0');
INSERT INTO `hotkeys` VALUES ('42', '2', '40', '0', '0');
INSERT INTO `hotkeys` VALUES ('43', '2', '41', '0', '0');
INSERT INTO `hotkeys` VALUES ('44', '2', '42', '0', '0');
INSERT INTO `hotkeys` VALUES ('45', '2', '43', '0', '0');
INSERT INTO `hotkeys` VALUES ('46', '2', '44', '0', '0');
INSERT INTO `hotkeys` VALUES ('47', '2', '45', '0', '0');
INSERT INTO `hotkeys` VALUES ('48', '2', '46', '0', '0');
INSERT INTO `hotkeys` VALUES ('49', '2', '47', '0', '0');
INSERT INTO `hotkeys` VALUES ('50', '2', '48', '0', '0');
INSERT INTO `hotkeys` VALUES ('51', '2', '49', '0', '0');
INSERT INTO `hotkeys` VALUES ('52', '2', '50', '0', '0');
INSERT INTO `hotkeys` VALUES ('53', '23', '0', '0', '0');
INSERT INTO `hotkeys` VALUES ('54', '23', '1', '0', '0');
INSERT INTO `hotkeys` VALUES ('55', '23', '2', '0', '0');
INSERT INTO `hotkeys` VALUES ('56', '23', '3', '0', '0');
INSERT INTO `hotkeys` VALUES ('57', '23', '4', '0', '0');
INSERT INTO `hotkeys` VALUES ('58', '23', '5', '0', '0');
INSERT INTO `hotkeys` VALUES ('59', '23', '6', '0', '0');
INSERT INTO `hotkeys` VALUES ('60', '23', '7', '0', '0');
INSERT INTO `hotkeys` VALUES ('61', '23', '8', '0', '0');
INSERT INTO `hotkeys` VALUES ('62', '23', '9', '0', '0');
INSERT INTO `hotkeys` VALUES ('63', '23', '10', '0', '0');
INSERT INTO `hotkeys` VALUES ('64', '23', '11', '0', '0');
INSERT INTO `hotkeys` VALUES ('65', '23', '12', '0', '0');
INSERT INTO `hotkeys` VALUES ('66', '23', '13', '0', '0');
INSERT INTO `hotkeys` VALUES ('67', '23', '14', '0', '0');
INSERT INTO `hotkeys` VALUES ('68', '23', '15', '0', '0');
INSERT INTO `hotkeys` VALUES ('69', '23', '16', '0', '0');
INSERT INTO `hotkeys` VALUES ('70', '23', '17', '0', '0');
INSERT INTO `hotkeys` VALUES ('71', '23', '18', '0', '0');
INSERT INTO `hotkeys` VALUES ('72', '23', '19', '0', '0');
INSERT INTO `hotkeys` VALUES ('73', '23', '20', '0', '0');
INSERT INTO `hotkeys` VALUES ('74', '23', '21', '0', '0');
INSERT INTO `hotkeys` VALUES ('75', '23', '22', '0', '0');
INSERT INTO `hotkeys` VALUES ('76', '23', '23', '0', '0');
INSERT INTO `hotkeys` VALUES ('77', '23', '24', '0', '0');
INSERT INTO `hotkeys` VALUES ('78', '23', '25', '0', '0');
INSERT INTO `hotkeys` VALUES ('79', '23', '26', '0', '0');
INSERT INTO `hotkeys` VALUES ('80', '23', '27', '0', '0');
INSERT INTO `hotkeys` VALUES ('81', '23', '28', '0', '0');
INSERT INTO `hotkeys` VALUES ('82', '23', '29', '0', '0');
INSERT INTO `hotkeys` VALUES ('83', '23', '30', '0', '0');
INSERT INTO `hotkeys` VALUES ('84', '23', '31', '0', '0');
INSERT INTO `hotkeys` VALUES ('85', '23', '32', '0', '0');
INSERT INTO `hotkeys` VALUES ('86', '23', '33', '0', '0');
INSERT INTO `hotkeys` VALUES ('87', '23', '34', '0', '0');
INSERT INTO `hotkeys` VALUES ('88', '23', '35', '0', '0');
INSERT INTO `hotkeys` VALUES ('89', '23', '36', '0', '0');
INSERT INTO `hotkeys` VALUES ('90', '23', '37', '0', '0');
INSERT INTO `hotkeys` VALUES ('91', '23', '38', '0', '0');
INSERT INTO `hotkeys` VALUES ('92', '23', '39', '0', '0');
INSERT INTO `hotkeys` VALUES ('93', '23', '40', '0', '0');
INSERT INTO `hotkeys` VALUES ('94', '23', '41', '0', '0');
INSERT INTO `hotkeys` VALUES ('95', '23', '42', '0', '0');
INSERT INTO `hotkeys` VALUES ('96', '23', '43', '0', '0');
INSERT INTO `hotkeys` VALUES ('97', '23', '44', '0', '0');
INSERT INTO `hotkeys` VALUES ('98', '23', '45', '0', '0');
INSERT INTO `hotkeys` VALUES ('99', '23', '46', '0', '0');
INSERT INTO `hotkeys` VALUES ('100', '23', '47', '0', '0');
INSERT INTO `hotkeys` VALUES ('101', '23', '48', '0', '0');
INSERT INTO `hotkeys` VALUES ('102', '23', '49', '0', '0');
INSERT INTO `hotkeys` VALUES ('103', '23', '50', '0', '0');
INSERT INTO `hotkeys` VALUES ('104', '5', '0', '0', '0');
INSERT INTO `hotkeys` VALUES ('105', '5', '1', '0', '0');
INSERT INTO `hotkeys` VALUES ('106', '5', '2', '0', '0');
INSERT INTO `hotkeys` VALUES ('107', '5', '3', '0', '0');
INSERT INTO `hotkeys` VALUES ('108', '5', '4', '0', '0');
INSERT INTO `hotkeys` VALUES ('109', '5', '5', '0', '0');
INSERT INTO `hotkeys` VALUES ('110', '5', '6', '0', '0');
INSERT INTO `hotkeys` VALUES ('111', '5', '7', '0', '0');
INSERT INTO `hotkeys` VALUES ('112', '5', '8', '0', '0');
INSERT INTO `hotkeys` VALUES ('113', '5', '9', '0', '0');
INSERT INTO `hotkeys` VALUES ('114', '5', '10', '0', '0');
INSERT INTO `hotkeys` VALUES ('115', '5', '11', '0', '0');
INSERT INTO `hotkeys` VALUES ('116', '5', '12', '0', '0');
INSERT INTO `hotkeys` VALUES ('117', '5', '13', '0', '0');
INSERT INTO `hotkeys` VALUES ('118', '5', '14', '0', '0');
INSERT INTO `hotkeys` VALUES ('119', '5', '15', '0', '0');
INSERT INTO `hotkeys` VALUES ('120', '5', '16', '0', '0');
INSERT INTO `hotkeys` VALUES ('121', '5', '17', '0', '0');
INSERT INTO `hotkeys` VALUES ('122', '5', '18', '0', '0');
INSERT INTO `hotkeys` VALUES ('123', '5', '19', '0', '0');
INSERT INTO `hotkeys` VALUES ('124', '5', '20', '0', '0');
INSERT INTO `hotkeys` VALUES ('125', '5', '21', '0', '0');
INSERT INTO `hotkeys` VALUES ('126', '5', '22', '0', '0');
INSERT INTO `hotkeys` VALUES ('127', '5', '23', '0', '0');
INSERT INTO `hotkeys` VALUES ('128', '5', '24', '0', '0');
INSERT INTO `hotkeys` VALUES ('129', '5', '25', '0', '0');
INSERT INTO `hotkeys` VALUES ('130', '5', '26', '0', '0');
INSERT INTO `hotkeys` VALUES ('131', '5', '27', '0', '0');
INSERT INTO `hotkeys` VALUES ('132', '5', '28', '0', '0');
INSERT INTO `hotkeys` VALUES ('133', '5', '29', '0', '0');
INSERT INTO `hotkeys` VALUES ('134', '5', '30', '0', '0');
INSERT INTO `hotkeys` VALUES ('135', '5', '31', '0', '0');
INSERT INTO `hotkeys` VALUES ('136', '5', '32', '0', '0');
INSERT INTO `hotkeys` VALUES ('137', '5', '33', '0', '0');
INSERT INTO `hotkeys` VALUES ('138', '5', '34', '0', '0');
INSERT INTO `hotkeys` VALUES ('139', '5', '35', '0', '0');
INSERT INTO `hotkeys` VALUES ('140', '5', '36', '0', '0');
INSERT INTO `hotkeys` VALUES ('141', '5', '37', '0', '0');
INSERT INTO `hotkeys` VALUES ('142', '5', '38', '0', '0');
INSERT INTO `hotkeys` VALUES ('143', '5', '39', '0', '0');
INSERT INTO `hotkeys` VALUES ('144', '5', '40', '0', '0');
INSERT INTO `hotkeys` VALUES ('145', '5', '41', '0', '0');
INSERT INTO `hotkeys` VALUES ('146', '5', '42', '0', '0');
INSERT INTO `hotkeys` VALUES ('147', '5', '43', '0', '0');
INSERT INTO `hotkeys` VALUES ('148', '5', '44', '0', '0');
INSERT INTO `hotkeys` VALUES ('149', '5', '45', '0', '0');
INSERT INTO `hotkeys` VALUES ('150', '5', '46', '0', '0');
INSERT INTO `hotkeys` VALUES ('151', '5', '47', '0', '0');
INSERT INTO `hotkeys` VALUES ('152', '5', '48', '0', '0');
INSERT INTO `hotkeys` VALUES ('153', '5', '49', '0', '0');
INSERT INTO `hotkeys` VALUES ('154', '5', '50', '0', '0');
INSERT INTO `hotkeys` VALUES ('155', '5', '0', '0', '0');
INSERT INTO `hotkeys` VALUES ('156', '5', '1', '0', '0');
INSERT INTO `hotkeys` VALUES ('157', '5', '2', '0', '0');
INSERT INTO `hotkeys` VALUES ('158', '5', '3', '0', '0');
INSERT INTO `hotkeys` VALUES ('159', '5', '4', '0', '0');
INSERT INTO `hotkeys` VALUES ('160', '5', '5', '0', '0');
INSERT INTO `hotkeys` VALUES ('161', '5', '6', '0', '0');
INSERT INTO `hotkeys` VALUES ('162', '5', '7', '0', '0');
INSERT INTO `hotkeys` VALUES ('163', '5', '8', '0', '0');
INSERT INTO `hotkeys` VALUES ('164', '5', '9', '0', '0');
INSERT INTO `hotkeys` VALUES ('165', '5', '10', '0', '0');
INSERT INTO `hotkeys` VALUES ('166', '5', '11', '0', '0');
INSERT INTO `hotkeys` VALUES ('167', '5', '12', '0', '0');
INSERT INTO `hotkeys` VALUES ('168', '5', '13', '0', '0');
INSERT INTO `hotkeys` VALUES ('169', '5', '14', '0', '0');
INSERT INTO `hotkeys` VALUES ('170', '5', '15', '0', '0');
INSERT INTO `hotkeys` VALUES ('171', '5', '16', '0', '0');
INSERT INTO `hotkeys` VALUES ('172', '5', '17', '0', '0');
INSERT INTO `hotkeys` VALUES ('173', '5', '18', '0', '0');
INSERT INTO `hotkeys` VALUES ('174', '5', '19', '0', '0');
INSERT INTO `hotkeys` VALUES ('175', '5', '20', '0', '0');
INSERT INTO `hotkeys` VALUES ('176', '5', '21', '0', '0');
INSERT INTO `hotkeys` VALUES ('177', '5', '22', '0', '0');
INSERT INTO `hotkeys` VALUES ('178', '5', '23', '0', '0');
INSERT INTO `hotkeys` VALUES ('179', '5', '24', '0', '0');
INSERT INTO `hotkeys` VALUES ('180', '5', '25', '0', '0');
INSERT INTO `hotkeys` VALUES ('181', '5', '26', '0', '0');
INSERT INTO `hotkeys` VALUES ('182', '5', '27', '0', '0');
INSERT INTO `hotkeys` VALUES ('183', '5', '28', '0', '0');
INSERT INTO `hotkeys` VALUES ('184', '5', '29', '0', '0');
INSERT INTO `hotkeys` VALUES ('185', '5', '30', '0', '0');
INSERT INTO `hotkeys` VALUES ('186', '5', '31', '0', '0');
INSERT INTO `hotkeys` VALUES ('187', '5', '32', '0', '0');
INSERT INTO `hotkeys` VALUES ('188', '5', '33', '0', '0');
INSERT INTO `hotkeys` VALUES ('189', '5', '34', '0', '0');
INSERT INTO `hotkeys` VALUES ('190', '5', '35', '0', '0');
INSERT INTO `hotkeys` VALUES ('191', '5', '36', '0', '0');
INSERT INTO `hotkeys` VALUES ('192', '5', '37', '0', '0');
INSERT INTO `hotkeys` VALUES ('193', '5', '38', '0', '0');
INSERT INTO `hotkeys` VALUES ('194', '5', '39', '0', '0');
INSERT INTO `hotkeys` VALUES ('195', '5', '40', '0', '0');
INSERT INTO `hotkeys` VALUES ('196', '5', '41', '0', '0');
INSERT INTO `hotkeys` VALUES ('197', '5', '42', '0', '0');
INSERT INTO `hotkeys` VALUES ('198', '5', '43', '0', '0');
INSERT INTO `hotkeys` VALUES ('199', '5', '44', '0', '0');
INSERT INTO `hotkeys` VALUES ('200', '5', '45', '0', '0');
INSERT INTO `hotkeys` VALUES ('201', '5', '46', '0', '0');
INSERT INTO `hotkeys` VALUES ('202', '5', '47', '0', '0');
INSERT INTO `hotkeys` VALUES ('203', '5', '48', '0', '0');
INSERT INTO `hotkeys` VALUES ('204', '5', '49', '0', '0');
INSERT INTO `hotkeys` VALUES ('205', '5', '50', '0', '0');
INSERT INTO `items` VALUES ('934', '0', '1', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('935', '0', '1', '0', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('936', '0', '1', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('937', '0', '1', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('938', '3644', '1', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('939', '3645', '1', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('940', '0', '1', '0', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('941', '0', '1', '0', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('942', '0', '1', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('943', '0', '1', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('944', '0', '1', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('945', '0', '1', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('946', '0', '1', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('947', '0', '1', '0', '13', '0', '30', 'item13');
INSERT INTO `items` VALUES ('948', '8', '1', '0', '14', '47', '30', 'item14');
INSERT INTO `items` VALUES ('949', '0', '1', '0', '15', '0', '30', 'item15');
INSERT INTO `items` VALUES ('950', '0', '1', '0', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('951', '8', '1', '0', '17', '47', '30', 'item17');
INSERT INTO `items` VALUES ('952', '0', '1', '0', '18', '0', '0', 'item18');
INSERT INTO `items` VALUES ('953', '3643', '1', '2', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('954', '0', '1', '0', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('955', '0', '1', '0', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('956', '0', '1', '0', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('957', '0', '1', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('958', '0', '1', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('959', '0', '1', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('960', '0', '1', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('961', '0', '1', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('962', '0', '1', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('963', '0', '1', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('964', '0', '1', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('965', '0', '1', '0', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('966', '0', '1', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('967', '0', '1', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('968', '0', '1', '0', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('969', '0', '1', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('970', '0', '1', '0', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('971', '0', '1', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('972', '0', '1', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('973', '0', '1', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('974', '0', '1', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('975', '0', '1', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('976', '0', '1', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('977', '0', '1', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('978', '0', '1', '0', '44', '0', '30', 'item44');
INSERT INTO `items` VALUES ('979', '0', '2', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('980', '3643', '2', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('981', '0', '2', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('982', '0', '2', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('983', '3644', '2', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('984', '3645', '2', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('985', '4055', '2', '5', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('986', '0', '2', '3', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('987', '0', '2', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('988', '0', '2', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('989', '0', '2', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('990', '0', '2', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('991', '0', '2', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('992', '118', '2', '5', '13', '0', '30', 'item13');
INSERT INTO `items` VALUES ('993', '3632', '2', '1', '14', '0', '30', 'item14');
INSERT INTO `items` VALUES ('994', '0', '2', '0', '15', '0', '0', 'item15');
INSERT INTO `items` VALUES ('995', '0', '2', '0', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('996', '0', '2', '0', '17', '0', '30', 'item17');
INSERT INTO `items` VALUES ('997', '5912', '2', '0', '18', '1000', '30', 'item18');
INSERT INTO `items` VALUES ('998', '0', '2', '0', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('999', '0', '2', '0', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('1000', '0', '2', '0', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('1001', '0', '2', '0', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('1002', '0', '2', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('1003', '0', '2', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('1004', '0', '2', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('1005', '0', '2', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('1006', '7', '2', '0', '27', '252', '30', 'item27');
INSERT INTO `items` VALUES ('1007', '0', '2', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('1008', '0', '2', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('1009', '0', '2', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('1010', '7', '2', '0', '31', '50', '30', 'item31');
INSERT INTO `items` VALUES ('1011', '0', '2', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('1012', '0', '2', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('1013', '0', '2', '0', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('1014', '0', '2', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('1015', '5912', '2', '0', '36', '505', '30', 'item36');
INSERT INTO `items` VALUES ('1016', '0', '2', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('1017', '0', '2', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('1018', '0', '2', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('1019', '0', '2', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('1020', '0', '2', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('1021', '0', '2', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('1022', '0', '2', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('1023', '0', '2', '0', '44', '0', '30', 'item44');
INSERT INTO `items` VALUES ('1024', '0', '23', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('1025', '11465', '23', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('1026', '0', '23', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('1027', '0', '23', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('1028', '11466', '23', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('1029', '11467', '23', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('1030', '10735', '23', '1', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('1031', '0', '23', '0', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('1032', '0', '23', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('1033', '0', '23', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('1034', '0', '23', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('1035', '0', '23', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('1036', '0', '23', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('1037', '0', '23', '0', '13', '0', '30', 'item13');
INSERT INTO `items` VALUES ('1038', '0', '23', '0', '14', '0', '30', 'item14');
INSERT INTO `items` VALUES ('1039', '0', '23', '0', '15', '0', '30', 'item15');
INSERT INTO `items` VALUES ('1040', '0', '23', '0', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('1041', '0', '23', '0', '17', '0', '30', 'item17');
INSERT INTO `items` VALUES ('1042', '0', '23', '0', '18', '0', '30', 'item18');
INSERT INTO `items` VALUES ('1043', '0', '23', '0', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('1044', '0', '23', '0', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('1045', '0', '23', '0', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('1046', '0', '23', '0', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('1047', '0', '23', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('1048', '0', '23', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('1049', '0', '23', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('1050', '0', '23', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('1051', '0', '23', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('1052', '0', '23', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('1053', '0', '23', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('1054', '0', '23', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('1055', '0', '23', '0', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('1056', '0', '23', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('1057', '0', '23', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('1058', '0', '23', '0', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('1059', '0', '23', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('1060', '0', '23', '0', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('1061', '0', '23', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('1062', '0', '23', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('1063', '0', '23', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('1064', '0', '23', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('1065', '0', '23', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('1066', '0', '23', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('1067', '0', '23', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('1068', '0', '23', '0', '44', '0', '30', 'item44');
INSERT INTO `items` VALUES ('1069', '0', '5', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('1070', '11462', '5', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('1071', '0', '5', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('1072', '0', '5', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('1073', '11463', '5', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('1074', '11464', '5', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('1075', '10733', '5', '1', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('1076', '10738', '5', '3', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('1077', '0', '5', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('1078', '0', '5', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('1079', '0', '5', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('1080', '0', '5', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('1081', '0', '5', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('1082', '0', '5', '0', '13', '0', '30', 'item13');
INSERT INTO `items` VALUES ('1083', '0', '5', '0', '14', '0', '30', 'item14');
INSERT INTO `items` VALUES ('1084', '0', '5', '0', '15', '0', '30', 'item15');
INSERT INTO `items` VALUES ('1085', '0', '5', '0', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('1086', '0', '5', '0', '17', '0', '30', 'item17');
INSERT INTO `items` VALUES ('1087', '0', '5', '0', '18', '0', '30', 'item18');
INSERT INTO `items` VALUES ('1088', '0', '5', '0', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('1089', '0', '5', '0', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('1090', '0', '5', '0', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('1091', '0', '5', '0', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('1092', '0', '5', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('1093', '0', '5', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('1094', '0', '5', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('1095', '0', '5', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('1096', '0', '5', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('1097', '0', '5', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('1098', '0', '5', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('1099', '0', '5', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('1100', '0', '5', '0', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('1101', '0', '5', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('1102', '0', '5', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('1103', '0', '5', '0', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('1104', '0', '5', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('1105', '0', '5', '0', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('1106', '0', '5', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('1107', '0', '5', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('1108', '0', '5', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('1109', '0', '5', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('1110', '0', '5', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('1111', '0', '5', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('1112', '0', '5', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('1113', '0', '5', '0', '44', '0', '30', 'item44');
INSERT INTO `items` VALUES ('1114', '0', '5', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('1115', '11462', '5', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('1116', '0', '5', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('1117', '0', '5', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('1118', '11463', '5', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('1119', '11464', '5', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('1120', '10733', '5', '1', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('1121', '0', '5', '0', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('1122', '0', '5', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('1123', '0', '5', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('1124', '0', '5', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('1125', '0', '5', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('1126', '0', '5', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('1127', '0', '5', '0', '13', '0', '30', 'item13');
INSERT INTO `items` VALUES ('1128', '0', '5', '0', '14', '0', '30', 'item14');
INSERT INTO `items` VALUES ('1129', '0', '5', '0', '15', '0', '30', 'item15');
INSERT INTO `items` VALUES ('1130', '0', '5', '0', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('1131', '0', '5', '0', '17', '0', '30', 'item17');
INSERT INTO `items` VALUES ('1132', '0', '5', '0', '18', '0', '30', 'item18');
INSERT INTO `items` VALUES ('1133', '0', '5', '0', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('1134', '0', '5', '0', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('1135', '0', '5', '0', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('1136', '0', '5', '0', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('1137', '0', '5', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('1138', '0', '5', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('1139', '0', '5', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('1140', '0', '5', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('1141', '0', '5', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('1142', '0', '5', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('1143', '0', '5', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('1144', '0', '5', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('1145', '0', '5', '0', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('1146', '0', '5', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('1147', '0', '5', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('1148', '0', '5', '0', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('1149', '0', '5', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('1150', '0', '5', '0', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('1151', '0', '5', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('1152', '0', '5', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('1153', '0', '5', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('1154', '0', '5', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('1155', '0', '5', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('1156', '0', '5', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('1157', '0', '5', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('1158', '0', '5', '0', '44', '0', '30', 'item44');
INSERT INTO `masteries` VALUES ('237', '1', '257', '0');
INSERT INTO `masteries` VALUES ('238', '1', '258', '0');
INSERT INTO `masteries` VALUES ('239', '1', '259', '0');
INSERT INTO `masteries` VALUES ('240', '1', '273', '0');
INSERT INTO `masteries` VALUES ('241', '1', '274', '0');
INSERT INTO `masteries` VALUES ('242', '1', '275', '0');
INSERT INTO `masteries` VALUES ('243', '1', '276', '0');
INSERT INTO `masteries` VALUES ('244', '2', '257', '6');
INSERT INTO `masteries` VALUES ('245', '2', '258', '0');
INSERT INTO `masteries` VALUES ('246', '2', '259', '0');
INSERT INTO `masteries` VALUES ('247', '2', '273', '0');
INSERT INTO `masteries` VALUES ('248', '2', '274', '0');
INSERT INTO `masteries` VALUES ('249', '2', '275', '0');
INSERT INTO `masteries` VALUES ('250', '2', '276', '0');
INSERT INTO `masteries` VALUES ('251', '23', '513', '0');
INSERT INTO `masteries` VALUES ('252', '23', '514', '0');
INSERT INTO `masteries` VALUES ('253', '23', '515', '0');
INSERT INTO `masteries` VALUES ('254', '23', '516', '0');
INSERT INTO `masteries` VALUES ('255', '23', '517', '0');
INSERT INTO `masteries` VALUES ('256', '23', '518', '0');
INSERT INTO `masteries` VALUES ('257', '5', '513', '0');
INSERT INTO `masteries` VALUES ('258', '5', '514', '0');
INSERT INTO `masteries` VALUES ('259', '5', '515', '0');
INSERT INTO `masteries` VALUES ('260', '5', '516', '0');
INSERT INTO `masteries` VALUES ('261', '5', '517', '0');
INSERT INTO `masteries` VALUES ('262', '5', '518', '0');
INSERT INTO `masteries` VALUES ('263', '5', '513', '0');
INSERT INTO `masteries` VALUES ('264', '5', '514', '0');
INSERT INTO `masteries` VALUES ('265', '5', '515', '0');
INSERT INTO `masteries` VALUES ('266', '5', '516', '0');
INSERT INTO `masteries` VALUES ('267', '5', '517', '0');
INSERT INTO `masteries` VALUES ('268', '5', '518', '0');
INSERT INTO `news` VALUES ('1', 'Opening', '<center><font color=red><b>Welcome to the Visual Silkroad Project</b></font></center>', '25', '4');
INSERT INTO `positions` VALUES ('1', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `positions` VALUES ('2', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `positions` VALUES ('5', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `positions` VALUES ('23', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `servers` VALUES ('3', 'Local', '0', '500', '1', '127.0.0.1', '15780');
INSERT INTO `servers` VALUES ('4', 'Remote', '0', '500', '1', '84.157.143.106', '15780');
INSERT INTO `skills` VALUES ('5', '2', '3');
INSERT INTO `users` VALUES ('3', 'i', 'i', '0', '0', 'You got banned for 10 Minutes because of 5 failed Logins.', '2010-10-01 16:01:38');
INSERT INTO `users` VALUES ('9', 'test', 'test', '0', '0', '...', '3000-01-01 00:00:00');
