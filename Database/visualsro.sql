/*
MySQL Data Transfer
Source Host: localhost
Source Database: visualsro
Target Host: localhost
Target Database: visualsro
Date: 11.10.2010 22:12:12
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
) ENGINE=InnoDB AUTO_INCREMENT=889 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=230 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `characters` VALUES ('1', '3', 'Over', '1907', '34', '100', '0', '20', '20', '0', '1421', '1421', '0', '1000-01-01 00:00:00', '950000', '1000000', '-1', '168', '97', '1026', '1289', '-32', '1421', '1421', '6', '9', '6', '6', '6', '3', '110', '110', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `characters` VALUES ('2', '2', 'Happy11', '14738', '65', '100', '0', '20', '20', '0', '1421', '1421', '0', '1000-01-01 00:00:00', '1000000', '1000000', '-1', '127', '104', '684', '1483', '538', '1421', '300', '6', '9', '6', '6', '6', '3', '11', '11', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `characters` VALUES ('3', '5', 'Nob', '1907', '34', '100', '0', '20', '20', '0', '1421', '1421', '0', '1000-01-01 00:00:00', '1049999', '999359', '-1', '168', '96', '1199', '1046', '65505', '200', '300', '6', '9', '6', '6', '6', '3', '110', '110', '16', '50', '100', '0', '255', '45', '1');
INSERT INTO `items` VALUES ('709', '4295', '1', '15', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('710', '4367', '1', '15', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('711', '4331', '1', '15', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('712', '4439', '1', '15', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('713', '4403', '1', '15', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('714', '4475', '1', '15', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('715', '11308', '1', '10', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('716', '0', '1', '0', '7', '0', '0', 'item7');
INSERT INTO `items` VALUES ('717', '0', '1', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('718', '0', '1', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('719', '0', '1', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('720', '0', '1', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('721', '0', '1', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('722', '0', '1', '0', '13', '0', '0', 'item13');
INSERT INTO `items` VALUES ('723', '3633', '1', '1', '14', '0', '30', 'item14');
INSERT INTO `items` VALUES ('724', '0', '1', '0', '15', '0', '0', 'item15');
INSERT INTO `items` VALUES ('725', '251', '1', '3', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('726', '11236', '1', '10', '17', '0', '30', 'item17');
INSERT INTO `items` VALUES ('727', '11200', '1', '15', '18', '0', '30', 'item18');
INSERT INTO `items` VALUES ('728', '15', '1', '0', '19', '44', '0', 'item19');
INSERT INTO `items` VALUES ('729', '15', '1', '0', '20', '46', '30', 'item20');
INSERT INTO `items` VALUES ('730', '0', '1', '0', '21', '0', '0', 'item21');
INSERT INTO `items` VALUES ('731', '2198', '1', '0', '22', '46', '30', 'item22');
INSERT INTO `items` VALUES ('732', '2199', '1', '0', '23', '41', '30', 'item23');
INSERT INTO `items` VALUES ('733', '61', '1', '0', '24', '46', '30', 'item24');
INSERT INTO `items` VALUES ('734', '0', '1', '0', '25', '0', '0', 'item25');
INSERT INTO `items` VALUES ('735', '0', '1', '0', '26', '0', '0', 'item26');
INSERT INTO `items` VALUES ('736', '0', '1', '0', '27', '0', '0', 'item27');
INSERT INTO `items` VALUES ('737', '0', '1', '0', '28', '0', '0', 'item28');
INSERT INTO `items` VALUES ('738', '0', '1', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('739', '5913', '1', '0', '30', '255', '0', 'item30');
INSERT INTO `items` VALUES ('740', '8', '1', '0', '31', '40', '0', 'item31');
INSERT INTO `items` VALUES ('741', '5912', '1', '0', '32', '255', '0', 'item32');
INSERT INTO `items` VALUES ('742', '3851', '1', '0', '33', '41', '30', 'item33');
INSERT INTO `items` VALUES ('743', '3851', '1', '0', '34', '50', '0', 'item34');
INSERT INTO `items` VALUES ('744', '24198', '1', '0', '35', '5', '30', 'item35');
INSERT INTO `items` VALUES ('745', '23441', '1', '0', '36', '1', '30', 'item36');
INSERT INTO `items` VALUES ('746', '3829', '1', '0', '37', '1', '30', 'item37');
INSERT INTO `items` VALUES ('747', '3828', '1', '0', '38', '50', '30', 'item38');
INSERT INTO `items` VALUES ('748', '3781', '1', '0', '39', '50', '0', 'item39');
INSERT INTO `items` VALUES ('749', '3775', '1', '0', '40', '50', '30', 'item40');
INSERT INTO `items` VALUES ('750', '23288', '1', '0', '41', '50', '30', 'item41');
INSERT INTO `items` VALUES ('751', '3795', '1', '0', '42', '48', '0', 'item42');
INSERT INTO `items` VALUES ('752', '3795', '1', '0', '43', '47', '30', 'item43');
INSERT INTO `items` VALUES ('753', '3851', '1', '0', '44', '46', '0', 'item44');
INSERT INTO `items` VALUES ('754', '0', '2', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('755', '11474', '2', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('756', '0', '2', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('757', '0', '2', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('758', '11475', '2', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('759', '11476', '2', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('760', '10735', '2', '1', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('761', '0', '2', '0', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('762', '0', '2', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('763', '0', '2', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('764', '0', '2', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('765', '0', '2', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('766', '0', '2', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('767', '108', '2', '5', '13', '0', '30', 'item13');
INSERT INTO `items` VALUES ('768', '21', '2', '0', '14', '50', '0', 'item14');
INSERT INTO `items` VALUES ('769', '8', '2', '0', '15', '50', '0', 'item15');
INSERT INTO `items` VALUES ('770', '0', '2', '0', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('771', '0', '2', '0', '17', '0', '0', 'item17');
INSERT INTO `items` VALUES ('772', '0', '2', '0', '18', '0', '0', 'item18');
INSERT INTO `items` VALUES ('773', '0', '2', '0', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('774', '0', '2', '0', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('775', '0', '2', '0', '21', '0', '0', 'item21');
INSERT INTO `items` VALUES ('776', '0', '2', '0', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('777', '0', '2', '0', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('778', '0', '2', '0', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('779', '0', '2', '0', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('780', '0', '2', '0', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('781', '0', '2', '0', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('782', '0', '2', '0', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('783', '0', '2', '0', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('784', '0', '2', '0', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('785', '0', '2', '0', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('786', '0', '2', '0', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('787', '0', '2', '0', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('788', '0', '2', '0', '34', '0', '0', 'item34');
INSERT INTO `items` VALUES ('789', '0', '2', '0', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('790', '0', '2', '0', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('791', '0', '2', '0', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('792', '0', '2', '0', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('793', '0', '2', '0', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('794', '0', '2', '0', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('795', '0', '2', '0', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('796', '0', '2', '0', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('797', '0', '2', '0', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('798', '0', '2', '0', '44', '0', '30', 'item44');
INSERT INTO `items` VALUES ('799', '0', '3', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('800', '3637', '3', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('801', '0', '3', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('802', '0', '3', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('803', '3638', '3', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('804', '3639', '3', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('805', '19619', '3', '9', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('806', '4223', '3', '15', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('807', '0', '3', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('808', '0', '3', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('809', '0', '3', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('810', '0', '3', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('811', '0', '3', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('812', '3851', '3', '0', '13', '45', '0', 'item13');
INSERT INTO `items` VALUES ('813', '19622', '3', '9', '14', '0', '30', 'item14');
INSERT INTO `items` VALUES ('814', '251', '3', '3', '15', '0', '30', 'item15');
INSERT INTO `items` VALUES ('815', '119', '3', '196', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('816', '119', '3', '255', '17', '0', '30', 'item17');
INSERT INTO `items` VALUES ('817', '134', '3', '255', '18', '0', '30', 'item18');
INSERT INTO `items` VALUES ('818', '134', '3', '255', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('819', '134', '3', '255', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('820', '134', '3', '255', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('821', '134', '3', '255', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('822', '134', '3', '255', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('823', '134', '3', '255', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('824', '134', '3', '255', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('825', '134', '3', '255', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('826', '134', '3', '255', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('827', '134', '3', '255', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('828', '134', '3', '255', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('829', '134', '3', '255', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('830', '134', '3', '255', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('831', '134', '3', '255', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('832', '134', '3', '255', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('833', '134', '3', '255', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('834', '134', '3', '255', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('835', '134', '3', '255', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('836', '134', '3', '255', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('837', '134', '3', '255', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('838', '134', '3', '255', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('839', '134', '3', '255', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('840', '134', '3', '255', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('841', '134', '3', '255', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('842', '134', '3', '255', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('843', '134', '3', '255', '44', '0', '30', 'item44');
INSERT INTO `items` VALUES ('844', '0', '3', '0', '0', '0', '30', 'item0');
INSERT INTO `items` VALUES ('845', '3637', '3', '2', '1', '0', '30', 'item1');
INSERT INTO `items` VALUES ('846', '0', '3', '0', '2', '0', '30', 'item2');
INSERT INTO `items` VALUES ('847', '0', '3', '0', '3', '0', '30', 'item3');
INSERT INTO `items` VALUES ('848', '3638', '3', '2', '4', '0', '30', 'item4');
INSERT INTO `items` VALUES ('849', '3639', '3', '2', '5', '0', '30', 'item5');
INSERT INTO `items` VALUES ('850', '19619', '3', '9', '6', '0', '30', 'item6');
INSERT INTO `items` VALUES ('851', '4223', '3', '15', '7', '0', '30', 'item7');
INSERT INTO `items` VALUES ('852', '0', '3', '0', '8', '0', '30', 'item8');
INSERT INTO `items` VALUES ('853', '0', '3', '0', '9', '0', '30', 'item9');
INSERT INTO `items` VALUES ('854', '0', '3', '0', '10', '0', '30', 'item10');
INSERT INTO `items` VALUES ('855', '0', '3', '0', '11', '0', '30', 'item11');
INSERT INTO `items` VALUES ('856', '0', '3', '0', '12', '0', '30', 'item12');
INSERT INTO `items` VALUES ('857', '3851', '3', '0', '13', '45', '0', 'item13');
INSERT INTO `items` VALUES ('858', '19622', '3', '9', '14', '0', '30', 'item14');
INSERT INTO `items` VALUES ('859', '251', '3', '3', '15', '0', '30', 'item15');
INSERT INTO `items` VALUES ('860', '119', '3', '196', '16', '0', '30', 'item16');
INSERT INTO `items` VALUES ('861', '119', '3', '255', '17', '0', '30', 'item17');
INSERT INTO `items` VALUES ('862', '134', '3', '255', '18', '0', '30', 'item18');
INSERT INTO `items` VALUES ('863', '134', '3', '255', '19', '0', '30', 'item19');
INSERT INTO `items` VALUES ('864', '134', '3', '255', '20', '0', '30', 'item20');
INSERT INTO `items` VALUES ('865', '134', '3', '255', '21', '0', '30', 'item21');
INSERT INTO `items` VALUES ('866', '134', '3', '255', '22', '0', '30', 'item22');
INSERT INTO `items` VALUES ('867', '134', '3', '255', '23', '0', '30', 'item23');
INSERT INTO `items` VALUES ('868', '134', '3', '255', '24', '0', '30', 'item24');
INSERT INTO `items` VALUES ('869', '134', '3', '255', '25', '0', '30', 'item25');
INSERT INTO `items` VALUES ('870', '134', '3', '255', '26', '0', '30', 'item26');
INSERT INTO `items` VALUES ('871', '134', '3', '255', '27', '0', '30', 'item27');
INSERT INTO `items` VALUES ('872', '134', '3', '255', '28', '0', '30', 'item28');
INSERT INTO `items` VALUES ('873', '134', '3', '255', '29', '0', '30', 'item29');
INSERT INTO `items` VALUES ('874', '134', '3', '255', '30', '0', '30', 'item30');
INSERT INTO `items` VALUES ('875', '134', '3', '255', '31', '0', '30', 'item31');
INSERT INTO `items` VALUES ('876', '134', '3', '255', '32', '0', '30', 'item32');
INSERT INTO `items` VALUES ('877', '134', '3', '255', '33', '0', '30', 'item33');
INSERT INTO `items` VALUES ('878', '134', '3', '255', '34', '0', '30', 'item34');
INSERT INTO `items` VALUES ('879', '134', '3', '255', '35', '0', '30', 'item35');
INSERT INTO `items` VALUES ('880', '134', '3', '255', '36', '0', '30', 'item36');
INSERT INTO `items` VALUES ('881', '134', '3', '255', '37', '0', '30', 'item37');
INSERT INTO `items` VALUES ('882', '134', '3', '255', '38', '0', '30', 'item38');
INSERT INTO `items` VALUES ('883', '134', '3', '255', '39', '0', '30', 'item39');
INSERT INTO `items` VALUES ('884', '134', '3', '255', '40', '0', '30', 'item40');
INSERT INTO `items` VALUES ('885', '134', '3', '255', '41', '0', '30', 'item41');
INSERT INTO `items` VALUES ('886', '134', '3', '255', '42', '0', '30', 'item42');
INSERT INTO `items` VALUES ('887', '134', '3', '255', '43', '0', '30', 'item43');
INSERT INTO `items` VALUES ('888', '134', '3', '255', '44', '0', '30', 'item44');
INSERT INTO `masteries` VALUES ('203', '1', '257', '0');
INSERT INTO `masteries` VALUES ('204', '1', '258', '0');
INSERT INTO `masteries` VALUES ('205', '1', '259', '0');
INSERT INTO `masteries` VALUES ('206', '1', '273', '0');
INSERT INTO `masteries` VALUES ('207', '1', '274', '0');
INSERT INTO `masteries` VALUES ('208', '1', '275', '0');
INSERT INTO `masteries` VALUES ('209', '1', '276', '0');
INSERT INTO `masteries` VALUES ('210', '2', '513', '0');
INSERT INTO `masteries` VALUES ('211', '2', '514', '0');
INSERT INTO `masteries` VALUES ('212', '2', '515', '0');
INSERT INTO `masteries` VALUES ('213', '2', '516', '0');
INSERT INTO `masteries` VALUES ('214', '2', '517', '0');
INSERT INTO `masteries` VALUES ('215', '2', '518', '0');
INSERT INTO `masteries` VALUES ('216', '3', '257', '23');
INSERT INTO `masteries` VALUES ('217', '3', '258', '0');
INSERT INTO `masteries` VALUES ('218', '3', '259', '0');
INSERT INTO `masteries` VALUES ('219', '3', '273', '0');
INSERT INTO `masteries` VALUES ('220', '3', '274', '13');
INSERT INTO `masteries` VALUES ('221', '3', '275', '0');
INSERT INTO `masteries` VALUES ('222', '3', '276', '0');
INSERT INTO `masteries` VALUES ('223', '3', '257', '23');
INSERT INTO `masteries` VALUES ('224', '3', '258', '0');
INSERT INTO `masteries` VALUES ('225', '3', '259', '0');
INSERT INTO `masteries` VALUES ('226', '3', '273', '0');
INSERT INTO `masteries` VALUES ('227', '3', '274', '13');
INSERT INTO `masteries` VALUES ('228', '3', '275', '0');
INSERT INTO `masteries` VALUES ('229', '3', '276', '0');
INSERT INTO `news` VALUES ('1', 'Opening', '<center><font color=red><b>Welcome to the Visual Silkroad Project</b></font></center>', '25', '4');
INSERT INTO `positions` VALUES ('1', '168', '97', '980', '1330', '65504', '135', '91', '1160', '1604', '243', '168', '97', '980', '1330', '65504');
INSERT INTO `positions` VALUES ('2', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `positions` VALUES ('3', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504', '168', '97', '980', '1330', '65504');
INSERT INTO `servers` VALUES ('3', 'Local', '0', '500', '1', '127.0.0.1', '15780');
INSERT INTO `servers` VALUES ('4', 'Remote', '0', '500', '1', '84.157.143.106', '15780');
INSERT INTO `skills` VALUES ('1', '3', '33');
INSERT INTO `skills` VALUES ('2', '3', '35');
INSERT INTO `skills` VALUES ('3', '3', '113');
INSERT INTO `skills` VALUES ('4', '3', '107');
INSERT INTO `users` VALUES ('2', 'test', 'test', '1', '0', '...', '3000-01-02');
INSERT INTO `users` VALUES ('3', 'i', 'i', '1', '0', 'You got banned by: __________12', '3000-01-01');
INSERT INTO `users` VALUES ('5', 'maex', 'maex', '0', '0', 'So halt^^', '3000-01-01');
