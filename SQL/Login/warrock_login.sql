/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50508
Source Host           : localhost:3306
Source Database       : warrock_login

Target Server Type    : MYSQL
Target Server Version : 50508
File Encoding         : 65001

Date: 2012-10-13 17:26:30
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `accounts`
-- ----------------------------
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts` (
  `UserID` bigint(20) NOT NULL,
  `NickName` varchar(255) NOT NULL,
  `username` varchar(255) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `Access_Level` tinyint(4) NOT NULL,
  `Bann_Time` bigint(4) NOT NULL DEFAULT '0',
  `Banned` tinyint(4) NOT NULL,
  `IsOnline` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`,`username`,`NickName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of accounts
-- ----------------------------
INSERT INTO `accounts` VALUES ('99999', 'Kacke', 'Lol', 'lol', '12', '0', '0', '0');

-- ----------------------------
-- Procedure structure for `AccountInfo_Save`
-- ----------------------------
DROP PROCEDURE IF EXISTS `AccountInfo_Save`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AccountInfo_Save`(IN `pUserID` bigint,IN `pIsOnline` tinyint,IN `pNickName` varchar(30),IN `pBanned` tinyint,IN `pBannTime` bigint)
proc_label:BEGIN
START TRANSACTION;
UPDATE Accounts SET IsOnline=pIsOnline,NickName=pNickName,Banned=pBanned,Bann_Time=pBannTime WHERE UserID=pUserID;
COMMIT;
IF @ROWCOUNT = 0 Then
   ROLLBACK;
        LEAVE proc_label;
END IF;
END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `unban_User`
-- ----------------------------
DROP PROCEDURE IF EXISTS `unban_User`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `unban_User`(IN `pUserID` bigint)
proc_label:BEGIN
START TRANSACTION;
UPDATE accounts SET Bann_Time=0,Banned=0 WHERE UserID=pUserID;
COMMIT;
IF @ROWCOUNT = 0 Then
   ROLLBACK;
        LEAVE proc_label;
END IF;
END
;;
DELIMITER ;
