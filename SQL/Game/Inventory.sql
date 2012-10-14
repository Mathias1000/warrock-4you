/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50508
Source Host           : localhost:3306
Source Database       : warrock_game

Target Server Type    : MYSQL
Target Server Version : 50508
File Encoding         : 65001

Date: 2012-10-14 23:26:14
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `inventory`
-- ----------------------------
DROP TABLE IF EXISTS `inventory`;
CREATE TABLE `inventory` (
  `UserID` bigint(20) NOT NULL,
  `ItemCode` varchar(255) NOT NULL,
  `expireDate` int(35) NOT NULL,
  `Class` tinyint(2) NOT NULL,
  `BandageSlot` tinyint(1) NOT NULL,
  `IsPX` tinyint(4) NOT NULL,
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of inventory
-- ----------------------------
INSERT INTO `inventory` VALUES ('99999', 'DF06', '0', '1', '2', '1');
