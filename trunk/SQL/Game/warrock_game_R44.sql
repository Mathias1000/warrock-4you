/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1_3306
Source Server Version : 50516
Source Host           : 127.0.0.1:3306
Source Database       : warrock_game

Target Server Type    : MYSQL
Target Server Version : 50516
File Encoding         : 65001

Date: 2012-10-26 17:10:11
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `account_details`
-- ----------------------------
DROP TABLE IF EXISTS `account_details`;
CREATE TABLE `account_details` (
  `PlayerID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `Experience` int(11) NOT NULL DEFAULT '0',
  `Level` tinyint(4) NOT NULL DEFAULT '1',
  `Dinar` int(11) NOT NULL DEFAULT '0',
  `Kills` int(11) NOT NULL DEFAULT '0',
  `Deaths` int(11) NOT NULL DEFAULT '0',
  `Copons` int(11) NOT NULL DEFAULT '0',
  `Cash` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`,`PlayerID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of account_details
-- ----------------------------
INSERT INTO `account_details` VALUES ('0', '90', '0', '1', '0', '0', '0', '0', '0');
INSERT INTO `account_details` VALUES ('0', '99999', '0', '2', '899982500', '0', '1', '0', '9000000');

-- ----------------------------
-- Table structure for `costumes`
-- ----------------------------
DROP TABLE IF EXISTS `costumes`;
CREATE TABLE `costumes` (
  `UserID` bigint(20) NOT NULL,
  `Class` tinyint(4) NOT NULL,
  `BandageCode` varchar(255) NOT NULL,
  `Equipt` smallint(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of costumes
-- ----------------------------

-- ----------------------------
-- Table structure for `costume_data`
-- ----------------------------
DROP TABLE IF EXISTS `costume_data`;
CREATE TABLE `costume_data` (
  `Code` varchar(255) NOT NULL,
  `Class` smallint(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of costume_data
-- ----------------------------

-- ----------------------------
-- Table structure for `equipment`
-- ----------------------------
DROP TABLE IF EXISTS `equipment`;
CREATE TABLE `equipment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `Slot` tinyint(4) NOT NULL,
  `Class` varchar(40) NOT NULL DEFAULT '',
  `ItemCode` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of equipment
-- ----------------------------
INSERT INTO `equipment` VALUES ('8', '99999', '3', '0', '^');
INSERT INTO `equipment` VALUES ('9', '99999', '1', '0', 'DB01');
INSERT INTO `equipment` VALUES ('10', '99999', '2', '0', 'DF01');
INSERT INTO `equipment` VALUES ('11', '99999', '3', '2', '^');
INSERT INTO `equipment` VALUES ('12', '99999', '1', '2', 'DB01');
INSERT INTO `equipment` VALUES ('13', '99999', '2', '2', '^');

-- ----------------------------
-- Table structure for `inventory`
-- ----------------------------
DROP TABLE IF EXISTS `inventory`;
CREATE TABLE `inventory` (
  `UserID` bigint(20) NOT NULL,
  `ItemCode` varchar(255) NOT NULL,
  `expireDate` int(35) NOT NULL DEFAULT '0',
  `InventorySlot` tinyint(1) NOT NULL DEFAULT '0',
  `IsPX` tinyint(4) NOT NULL DEFAULT '0',
  `Class0` tinyint(4) NOT NULL DEFAULT '0',
  `Class1` tinyint(4) NOT NULL DEFAULT '0',
  `Class2` tinyint(4) NOT NULL DEFAULT '0',
  `Class3` tinyint(4) NOT NULL DEFAULT '0',
  `Class4` tinyint(4) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of inventory
-- ----------------------------
INSERT INTO `inventory` VALUES ('99999', 'DA04', '0', '0', '0', '0', '0', '0', '0', '0');

-- ----------------------------
-- Table structure for `item_data`
-- ----------------------------
DROP TABLE IF EXISTS `item_data`;
CREATE TABLE `item_data` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `ItemCode` varchar(255) NOT NULL DEFAULT '^',
  `WiD` smallint(6) NOT NULL DEFAULT '0',
  `Name` varchar(255) NOT NULL DEFAULT '',
  `Damage` int(11) NOT NULL DEFAULT '0',
  `OnlyPremium` enum('false','true') NOT NULL DEFAULT 'false',
  `ValidShop` enum('false','true') NOT NULL DEFAULT 'false',
  `ValidPX` enum('false','true') NOT NULL DEFAULT 'false',
  `Price0` int(11) NOT NULL DEFAULT '0',
  `Price1` int(11) NOT NULL DEFAULT '0',
  `Price2` int(11) NOT NULL DEFAULT '0',
  `Price3` int(11) NOT NULL DEFAULT '0',
  `ValidWeapon` enum('false','true') NOT NULL DEFAULT 'false',
  `Class0` tinyint(4) NOT NULL DEFAULT '10',
  `Class1` tinyint(4) NOT NULL DEFAULT '10',
  `Class2` tinyint(4) NOT NULL DEFAULT '10',
  `Class3` tinyint(4) NOT NULL DEFAULT '10',
  `Class4` tinyint(4) NOT NULL DEFAULT '10',
  `Slot0` tinyint(4) NOT NULL DEFAULT '0',
  `Slot1` tinyint(4) NOT NULL DEFAULT '0',
  `Slot2` tinyint(4) NOT NULL DEFAULT '0',
  `Slot3` tinyint(4) NOT NULL DEFAULT '0',
  `Slot4` tinyint(4) NOT NULL DEFAULT '0',
  `Slot5` tinyint(4) NOT NULL DEFAULT '0',
  `Slot6` tinyint(4) NOT NULL DEFAULT '0',
  `Slot7` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of item_data
-- ----------------------------
INSERT INTO `item_data` VALUES ('2', 'DA04', '0', 'Katanatest', '0', 'false', 'true', 'false', '2500', '2500', '2500', '2500', 'false', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');

-- ----------------------------
-- Procedure structure for `details_Save`
-- ----------------------------
DROP PROCEDURE IF EXISTS `details_Save`;
DELIMITER ;;
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `details_Save`(IN `pUserID` bigint,IN `pExp` int,IN `pLevel` smallint,IN `pDinar` int,IN `pDeaths` int,IN `pCopons` int,IN `pCash` int)
proc_label:BEGIN
START TRANSACTION;
UPDATE account_details SET  Experience=pExp,Level=pLevel,Dinar=pDinar,Deaths=pDeaths,Copons=pCopons,Cash=pCash WHERE UserID=pUserID;
COMMIT;
IF @ROWCOUNT = 0 Then
   ROLLBACK;
        LEAVE proc_label;
END IF;
END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `Save_equipt`
-- ----------------------------
DROP PROCEDURE IF EXISTS `Save_equipt`;
DELIMITER ;;
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `Save_equipt`(IN `pUserID` bigint,IN `pItemCode` varchar(255),IN `pClass` tinyint,IN `pSlot` tinyint)
proc_label:BEGIN
if not exists(SELECT * FROM equipment WHERE UserID=pUserID AND Slot=pSlot AND Class=pClass) Then
START TRANSACTION;
INSERT INTO equipment (UserID,ItemCode,Class,Slot) VALUES (pUserID,pItemCode,pClass,pSlot);
COMMIT;
else
START TRANSACTION;
UPDATE equipment SET  ItemCode=pItemCode WHERE UserID=pUserID AND Slot=pSlot AND Class=pClass;
COMMIT;
        LEAVE proc_label;
END IF;
END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `Save_Item`
-- ----------------------------
DROP PROCEDURE IF EXISTS `Save_Item`;
DELIMITER ;;
CREATE DEFINER=`root`@`127.0.0.1` PROCEDURE `Save_Item`(IN `pUserID` bigint,IN `pIsPX` tinyint,IN `pClass0` tinyint,IN `pClass1` tinyint,IN `pClass2` tinyint,IN `pClass3` tinyint,IN `pClass4` tinyint,IN `pInventorySlot` tinyint,IN `pexpireDate` bigint,IN `pItemCode` varchar(255))
proc_label:BEGIN
if not exists(SELECT * FROM inventory WHERE UserID=pUserID AND InventorySlot=pInventorySlot) Then
START TRANSACTION;
INSERT INTO inventory (UserID,ItemCode,expireDate,InventorySlot,IsPX,Class0,Class1,Class2,Class3,Class4) 
VALUES (pUserID,pItemCode,pexpireDate,pInventorySlot,pIsPX,pClass0,pClass1,pClass2,pClass3,pClass4);
COMMIT;
else
START TRANSACTION;
UPDATE Inventory SET  expireDate=pexpireDate,ItemCode=pItemCode,IsPX=pIsPX,Class0=pClass0,Class1=pClass1,Class2=pClass2,Class3=pClass3,Class4=pClass4 WHERE UserID=pUserID AND InventorySlot=pInventorySlot;
COMMIT;
        LEAVE proc_label;
END IF;
END
;;
DELIMITER ;
