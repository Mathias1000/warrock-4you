/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50508
Source Host           : localhost:3306
Source Database       : warrock_game

Target Server Type    : MYSQL
Target Server Version : 50508
File Encoding         : 65001

Date: 2012-10-13 17:26:40
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
INSERT INTO `account_details` VALUES ('0', '23', '0', '1', '0', '0', '0', '0', '0');
INSERT INTO `account_details` VALUES ('0', '99999', '0', '2', '90', '0', '1', '0', '0');

-- ----------------------------
-- Procedure structure for `details_Save`
-- ----------------------------
DROP PROCEDURE IF EXISTS `details_Save`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `details_Save`(IN `pUserID` bigint,IN `pExp` int,IN `pLevel` smallint,IN `pDinar` int,IN `pDeaths` int,IN `pCopons` int,IN `pCash` int)
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
