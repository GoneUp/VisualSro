/*
Navicat MySQL Data Transfer

Source Server         : Daxter-PC
Source Server Version : 50513
Source Host           : localhost:3306
Source Database       : scse

Target Server Type    : MYSQL
Target Server Version : 50513
File Encoding         : 65001

Date: 2011-08-04 21:17:19
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `_refmobtype`
-- ----------------------------
DROP TABLE IF EXISTS `_refmobtype`;
CREATE TABLE `_refmobtype` (
  `Type` int(11) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `HP-Mult` varchar(255) DEFAULT NULL,
  `XP-Mult` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of _refmobtype
-- ----------------------------
INSERT INTO `_refmobtype` VALUES ('0', 'Normal', '1', '1');
INSERT INTO `_refmobtype` VALUES ('1', 'Champion', '2', '2');
INSERT INTO `_refmobtype` VALUES ('3', 'Unique', '1 if(Normal then 120)', '1 if(Normal then 90)');
INSERT INTO `_refmobtype` VALUES ('4', 'Giant', '20', '15');
INSERT INTO `_refmobtype` VALUES ('5', 'Titan', '100', '80 (Not sure)');
INSERT INTO `_refmobtype` VALUES ('6', 'Elite', '4', '4');
INSERT INTO `_refmobtype` VALUES ('8', 'Unique (less HP)', '0.5', '0.5');
INSERT INTO `_refmobtype` VALUES ('16', 'Normal(Party)', '10', '7.5');
INSERT INTO `_refmobtype` VALUES ('17', 'Champion(Party)', '20', '15');
INSERT INTO `_refmobtype` VALUES ('19', 'Unique(Party)', '2 if(Normal then 1200)', '2 if(Normal then 675)');
INSERT INTO `_refmobtype` VALUES ('20', 'Giant(Party)', '200', '112,5');
INSERT INTO `_refmobtype` VALUES ('21', 'Titan (Party)', '1000', '600');
INSERT INTO `_refmobtype` VALUES ('22', 'Elite (Party)', '40', '30');

-- ----------------------------
-- Table structure for `account`
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(16) DEFAULT NULL,
  `Password` varchar(16) DEFAULT NULL,
  `Level` int(1) DEFAULT '0' COMMENT 'Account Level: 0=Player,1=VIP,2=Assistent,3=Trial GM,4=GM,5=Head GM,6=Admin',
  `E-Mail` varchar(50) DEFAULT NULL,
  `SecretQuestion` varchar(50) DEFAULT NULL,
  `SecretAnswer` varchar(50) DEFAULT NULL,
  `RegistrationDate` datetime DEFAULT NULL,
  `RegistrationIP` varchar(15) DEFAULT NULL,
  `LastLoggedDate` datetime DEFAULT NULL,
  `LastLoggedIP` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of account
-- ----------------------------
INSERT INTO `account` VALUES ('5', 'Test', 'aAA', '0', null, null, null, null, null, null, null);

-- ----------------------------
-- Table structure for `account_ban`
-- ----------------------------
DROP TABLE IF EXISTS `account_ban`;
CREATE TABLE `account_ban` (
  `AccountID` int(11) NOT NULL DEFAULT '0',
  `CreatedBy` varchar(12) DEFAULT 'MySQL',
  `CreationDate` datetime DEFAULT '2000-01-01 00:00:00',
  `ExpirationDate` datetime DEFAULT '3000-01-01 00:00:00',
  `Reason` text,
  PRIMARY KEY (`AccountID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of account_ban
-- ----------------------------
INSERT INTO `account_ban` VALUES ('5', 'MySQL', '2000-01-01 00:00:00', '3000-01-01 00:00:00', 'Test');
INSERT INTO `account_ban` VALUES ('7', 'GM', '2011-08-02 00:01:24', null, 'Botting');

-- ----------------------------
-- Table structure for `account_silk`
-- ----------------------------
DROP TABLE IF EXISTS `account_silk`;
CREATE TABLE `account_silk` (
  `AccountID` int(11) NOT NULL DEFAULT '0',
  `Silk` int(11) DEFAULT '0',
  `DailySilk` int(11) DEFAULT '0',
  `MontlySilk` int(11) DEFAULT '0',
  `TotalSilk` int(11) DEFAULT '0',
  PRIMARY KEY (`AccountID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of account_silk
-- ----------------------------

-- ----------------------------
-- Table structure for `char`
-- ----------------------------
DROP TABLE IF EXISTS `char`;
CREATE TABLE `char` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `AccountID` int(11) DEFAULT NULL,
  `Name` varchar(12) DEFAULT NULL,
  `ObjectID` int(4) DEFAULT NULL,
  `Volume` tinyint(4) DEFAULT NULL,
  `CurLevel` tinyint(4) DEFAULT NULL,
  `MaxLevel` tinyint(4) DEFAULT NULL,
  `ExpOffset` bigint(20) DEFAULT NULL,
  `SExpOffset` int(11) DEFAULT NULL,
  `Str` int(11) DEFAULT NULL,
  `Int` int(11) DEFAULT NULL,
  `Gold` bigint(20) DEFAULT NULL,
  `SkillPoints` int(11) DEFAULT NULL,
  `StatPoints` int(11) DEFAULT NULL,
  `HwanCount` tinyint(4) DEFAULT NULL,
  `HP` int(11) DEFAULT NULL,
  `MP` int(11) DEFAULT NULL,
  `LatestRegion` int(11) DEFAULT NULL,
  `PosX` float DEFAULT NULL,
  `PosY` float DEFAULT NULL,
  `PosZ` float DEFAULT NULL,
  `AppointedTeleport` int(11) DEFAULT NULL,
  `InventorySize` tinyint(4) DEFAULT NULL,
  `DailyPK` tinyint(4) DEFAULT NULL,
  `TotalPK` smallint(6) DEFAULT NULL,
  `PKPenaltyPoint` int(11) DEFAULT NULL,
  `GuildID` int(11) DEFAULT NULL,
  `LastLogin` datetime DEFAULT NULL,
  `LastRegion` int(11) DEFAULT NULL,
  `LastPosX` float DEFAULT NULL,
  `LastPosY` float DEFAULT NULL,
  `LastPosZ` float DEFAULT NULL,
  `DiedRegion` int(11) DEFAULT NULL,
  `DiedPosX` float DEFAULT NULL,
  `DiedPosY` float DEFAULT NULL,
  `DiedPosZ` float DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of char
-- ----------------------------

-- ----------------------------
-- Table structure for `char_cos`
-- ----------------------------
DROP TABLE IF EXISTS `char_cos`;
CREATE TABLE `char_cos` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ObjectID` int(11) DEFAULT NULL,
  `HP` int(11) DEFAULT NULL,
  `MP` int(11) DEFAULT NULL,
  `State` int(1) DEFAULT '0',
  `Name` varchar(50) DEFAULT NULL,
  `Lvl` int(11) DEFAULT NULL,
  `ExpOffset` bigint(20) DEFAULT NULL,
  `HGP` int(11) DEFAULT NULL,
  `PetOption` int(3) DEFAULT NULL,
  `RentEndTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of char_cos
-- ----------------------------

-- ----------------------------
-- Table structure for `char_deleted`
-- ----------------------------
DROP TABLE IF EXISTS `char_deleted`;
CREATE TABLE `char_deleted` (
  `CharID` int(11) NOT NULL DEFAULT '0',
  `DeletedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`CharID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of char_deleted
-- ----------------------------

-- ----------------------------
-- Table structure for `char_mastery`
-- ----------------------------
DROP TABLE IF EXISTS `char_mastery`;
CREATE TABLE `char_mastery` (
  `CharID` int(11) DEFAULT '0',
  `MasteryID` int(3) DEFAULT NULL,
  `Level` int(3) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of char_mastery
-- ----------------------------

-- ----------------------------
-- Table structure for `char_skill`
-- ----------------------------
DROP TABLE IF EXISTS `char_skill`;
CREATE TABLE `char_skill` (
  `CharID` int(11) DEFAULT '0',
  `SkillID` int(11) DEFAULT NULL,
  `Enable` int(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of char_skill
-- ----------------------------

-- ----------------------------
-- Table structure for `guild`
-- ----------------------------
DROP TABLE IF EXISTS `guild`;
CREATE TABLE `guild` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Level` int(11) DEFAULT '1',
  `GatheredSP` int(11) DEFAULT '0',
  `FondationDate` datetime DEFAULT NULL,
  `UnionID` int(11) DEFAULT '0',
  `CommentTitle` text,
  `Comment` text,
  `CurrentCrestID` int(11) DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of guild
-- ----------------------------

-- ----------------------------
-- Table structure for `guild_member`
-- ----------------------------
DROP TABLE IF EXISTS `guild_member`;
CREATE TABLE `guild_member` (
  `GuildID` int(11) NOT NULL DEFAULT '0',
  `CharID` int(11) NOT NULL DEFAULT '0',
  `MemberClass` int(2) DEFAULT NULL,
  `GP` int(11) DEFAULT '0',
  `JoinDate` datetime DEFAULT NULL,
  `Permission` int(11) DEFAULT '0',
  `Grandname` varchar(0) DEFAULT NULL,
  PRIMARY KEY (`CharID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of guild_member
-- ----------------------------

-- ----------------------------
-- Table structure for `inventory`
-- ----------------------------
DROP TABLE IF EXISTS `inventory`;
CREATE TABLE `inventory` (
  `CharID` int(11) DEFAULT NULL,
  `Slot` int(3) DEFAULT NULL,
  `ItemID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of inventory
-- ----------------------------

-- ----------------------------
-- Table structure for `inventory_avatar`
-- ----------------------------
DROP TABLE IF EXISTS `inventory_avatar`;
CREATE TABLE `inventory_avatar` (
  `CharID` int(11) DEFAULT NULL,
  `Slot` int(3) DEFAULT NULL,
  `ItemID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of inventory_avatar
-- ----------------------------

-- ----------------------------
-- Table structure for `inventory_cos`
-- ----------------------------
DROP TABLE IF EXISTS `inventory_cos`;
CREATE TABLE `inventory_cos` (
  `COSID` int(11) DEFAULT NULL,
  `Slot` int(3) DEFAULT NULL,
  `ItemID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of inventory_cos
-- ----------------------------

-- ----------------------------
-- Table structure for `items`
-- ----------------------------
DROP TABLE IF EXISTS `items`;
CREATE TABLE `items` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ObjectID` int(11) DEFAULT NULL,
  `Plus` int(11) DEFAULT NULL,
  `Variance` int(11) DEFAULT NULL,
  `Data` int(11) DEFAULT NULL COMMENT 'ITEM_CH/EU = Durability\r\nITEM_ETC = Amount\r\nPet=COS ID of COS Table',
  `CreatorName` varchar(0) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of items
-- ----------------------------

-- ----------------------------
-- Table structure for `storage`
-- ----------------------------
DROP TABLE IF EXISTS `storage`;
CREATE TABLE `storage` (
  `AccountID` int(11) NOT NULL DEFAULT '0',
  `Slot` int(3) DEFAULT NULL,
  `ItemID` int(11) DEFAULT NULL,
  PRIMARY KEY (`AccountID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of storage
-- ----------------------------

-- ----------------------------
-- Table structure for `storage_guild`
-- ----------------------------
DROP TABLE IF EXISTS `storage_guild`;
CREATE TABLE `storage_guild` (
  `GuildID` int(11) NOT NULL DEFAULT '0',
  `Slot` int(3) DEFAULT NULL,
  `ItemID` int(11) DEFAULT NULL,
  PRIMARY KEY (`GuildID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of storage_guild
-- ----------------------------

-- ----------------------------
-- Table structure for `union`
-- ----------------------------
DROP TABLE IF EXISTS `union`;
CREATE TABLE `union` (
  `ID` int(11) NOT NULL DEFAULT '0',
  `LeadGuild` int(11) DEFAULT NULL,
  `Ally1` int(11) DEFAULT '0',
  `Ally2` int(11) DEFAULT '0',
  `Ally3` int(11) DEFAULT '0',
  `Ally4` int(11) DEFAULT '0',
  `Ally5` int(11) DEFAULT '0',
  `Ally6` int(11) DEFAULT '0',
  `Ally7` int(11) DEFAULT '0',
  `FoundationDate` datetime DEFAULT NULL,
  `UnionCrestID` int(11) DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of union
-- ----------------------------

-- ----------------------------
-- Function structure for `BanAccount`
-- ----------------------------
DROP FUNCTION IF EXISTS `BanAccount`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `BanAccount`(`inAccountID` int,`inCreator` varchar(50),`inExpirationDate` datetime,`inReason` text) RETURNS int(1)
BEGIN
DECLARE vAccountID INT;
SELECT `ID` INTO vAccountID FROM `account` WHERE `ID` = `inAccountID`;
INSERT INTO `account_ban` (`AccountID`, `CreatedBy`, `CreationDate`,`ExpirationDate`, `Reason`) VALUES (`inAccountID`,`inCreator`,CURRENT_TIMESTAMP,`inExpirationDate`,`inReason`);
RETURN IFNULL(vAccountID,0);
END
;;
DELIMITER ;
