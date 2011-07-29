/*
MySQL Data Transfer
Source Host: localhost
Source Database: visualsro
Target Host: localhost
Target Database: visualsro
Date: 10.04.2011 15:05:16
*/

SET FOREIGN_KEY_CHECKS=0;
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
  `dead_xpos` int(10) unsigned NOT NULL DEFAULT '980',
  `dead_ypos` int(10) unsigned NOT NULL DEFAULT '1330',
  `dead_zpos` int(10) NOT NULL DEFAULT '65504',
  PRIMARY KEY (`OwnerCharID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


