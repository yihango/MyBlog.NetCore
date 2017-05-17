/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50717
Source Host           : 127.0.0.1:3306
Source Database       : my_blog_db

Target Server Type    : MYSQL
Target Server Version : 50717
File Encoding         : 65001

Date: 2017-05-16 09:55:06
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for post_tb
-- ----------------------------
DROP TABLE IF EXISTS `post_tb`;
CREATE TABLE `post_tb` (
  `post_id` varchar(255) NOT NULL,
  `post_title` varchar(255) DEFAULT NULL,
  `post_summary` varchar(255) DEFAULT NULL,
  `post_pub_time` datetime DEFAULT NULL,
  `post_pub_sortTime` varchar(255) DEFAULT NULL,
  `post_tags` varchar(255) DEFAULT NULL,
  `post_path` varchar(255) DEFAULT NULL,
  `post_pub_state` int(11) DEFAULT NULL,
  PRIMARY KEY (`post_id`),
  KEY `post_title` (`post_title`),
  KEY `post_pub_state` (`post_pub_state`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
