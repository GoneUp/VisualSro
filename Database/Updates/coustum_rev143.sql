/*
MySQL Data Transfer
Source Host: localhost
Source Database: visualsro
Target Host: localhost
Target Database: visualsro
Date: 20.04.2011 20:35:29
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for coustum
-- ----------------------------
DROP TABLE IF EXISTS `coustum`;
CREATE TABLE `coustum` (
  `auto_id` int(10) NOT NULL AUTO_INCREMENT,
  `ownerid` int(10) unsigned NOT NULL,
  `name` varchar(255) NOT NULL,
  `settings` varchar(255) NOT NULL,
  PRIMARY KEY (`auto_id`)
) ENGINE=InnoDB AUTO_INCREMENT=925 DEFAULT CHARSET=utf8;


