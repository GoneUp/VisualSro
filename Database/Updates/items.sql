/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50403
Source Host           : localhost:3306
Source Database       : visualsro

Target Server Type    : MYSQL
Target Server Version : 50403
File Encoding         : 65001

Date: 2011-01-30 18:21:28
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `items`
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
  `PerDurability` int(11) DEFAULT '0',
  `PerPhyRef` int(11) DEFAULT '0',
  `PerMagRef` int(11) DEFAULT '0',
  `PerPhyAtk` int(11) DEFAULT '0',
  `PerMagAtk` int(11) DEFAULT '0',
  `PerPhyDef` int(11) DEFAULT '0',
  `PerMagDef` int(11) DEFAULT '0',
  `PerBlock` int(11) DEFAULT '0',
  `PerCritical` int(11) DEFAULT '0',
  `PerAttackRate` int(11) DEFAULT '0',
  `PerParryRate` int(11) DEFAULT '0',
  `PerPhyAbs` int(11) DEFAULT '0',
  `PerMagAbs` int(11) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5652 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of items
-- ----------------------------
