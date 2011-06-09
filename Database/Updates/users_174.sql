/*
MySQL Data Transfer
Source Host: localhost
Source Database: visualsro
Target Host: localhost
Target Database: visualsro
Date: 09.06.2011 20:42:06
*/

SET FOREIGN_KEY_CHECKS=0;
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
  `admin` tinyint(1) unsigned NOT NULL DEFAULT '0' COMMENT 'Acces to SR_Admin Tool',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=516 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `users` VALUES ('1', 'i', 'i', '0', '0', '...', '3000-01-01 00:00:00', '42091', '1');
