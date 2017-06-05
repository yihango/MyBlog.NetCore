/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50717
Source Host           : 192.168.111.1:3306
Source Database       : my_blog_db

Target Server Type    : MYSQL
Target Server Version : 50717
File Encoding         : 65001

Date: 2017-06-05 19:08:10
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for post_tag_tb
-- ----------------------------
DROP TABLE IF EXISTS `post_tag_tb`;
CREATE TABLE `post_tag_tb` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `post_id` varchar(255) NOT NULL,
  `tag_name` varchar(255) NOT NULL,
  `pub_state` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `tag_name_fk` (`tag_name`),
  KEY `post_id_fk` (`post_id`),
  KEY `pub_state_fk` (`pub_state`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8;
