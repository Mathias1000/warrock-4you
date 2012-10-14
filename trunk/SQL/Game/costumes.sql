/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50508
Source Host           : localhost:3306
Source Database       : warrock_game

Target Server Type    : MYSQL
Target Server Version : 50508
File Encoding         : 65001

Date: 2012-10-14 23:26:09
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `costumes`
-- ----------------------------
DROP TABLE IF EXISTS `costumes`;
CREATE TABLE `costumes` (
  `UserID` bigint(20) NOT NULL,
  `Class` tinyint(4) NOT NULL,
  `BandageCode` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of costumes
-- ----------------------------
INSERT INTO `costumes` VALUES ('99999', '1', 'BA11');
