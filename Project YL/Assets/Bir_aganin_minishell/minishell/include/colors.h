/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   colors.h                                           :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/22 23:37:41 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/23 00:57:08 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#ifndef COLORS_H
# define COLORS_H

# define C_RESET  "\001\033[0m\002"
# define C_BLACK  "\001\033[30m\002"
# define C_RED    "\001\033[1;91m\002"
# define C_GREEN  "\001\033[32m\002"
# define C_YELLOW "\001\033[1;93m\002"
# define C_BLUE   "\001\033[34m\002"
# define C_MAGENTA "\001\033[35m\002"
# define C_CYAN   "\001\033[36m\002"
# define C_WHITE  "\001\033[37m\002"

// color + underline
# define C_BLACK_UL   "\001\033[4;30m\002"
# define C_RED_UL     "\001\033[4;31m\002"
# define C_GREEN_UL   "\001\033[4;32m\002"
# define C_YELLOW_UL  "\001\033[4;33m\002"
# define C_BLUE_UL    "\001\033[4;34m\002"
# define C_MAGENTA_UL "\001\033[4;35m\002"
# define C_CYAN_UL    "\001\033[4;36m\002"
# define C_WHITE_UL   "\001\033[4;37m\002"

// color + underline + background
# define C_BLACK_UL_BG   "\001\033[1;4;30;40m\002"
# define C_RED_UL_BG     "\001\033[1;4;91;41m\002"
# define C_GREEN_UL_BG   "\001\033[1;4;32;42m\002"
# define C_YELLOW_UL_BG  "\001\033[1;4;33;43m\002"
# define C_BLUE_UL_BG    "\001\033[1;4;34;44m\002"
# define C_MAGENTA_UL_BG "\001\033[1;4;35;45m\002"
# define C_CYAN_UL_BG    "\001\033[1;4;36;46m\002"
# define C_WHITE_UL_BG   "\001\033[1;4;37;47m\002"

#endif
