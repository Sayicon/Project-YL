/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   boolean.h                                          :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2024/12/26 00:41:37 by caide-so          #+#    #+#             */
/*   Updated: 2025/02/05 20:05:45 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#ifndef BOOLEAN_H
# define BOOLEAN_H

/* @brief Checks if the int 'c' is an alphabetical char from the ASCII table. 
 *
 * @param c the character
 * @return int 0 if false and 1 if true.
*/
int		ft_isalpha(int c);

/* @brief Checks if the int 'c' is a digit char from the ASCII table. 
 *
 * @param c the character
 * @return int 0 if false and 1 if true.
*/
int		ft_isdigit(int c);

/* @brief Checks if the int 'c' is an alphanumeric char from the ASCII table. 
 *
 * @param c the character
 * @return int 0 if false and 1 if true.
*/
int		ft_isalnum(int c);

/* @brief Checks if the int 'c' is in the ASCII table. 
 *
 * @param c the character
 * @return int 0 if false and 1 if true.
*/
int		ft_isascii(int c);

/* @brief Checks if the int 'c' is printable. 
 *
 * @param c the character
 * @return int 0 if false and 1 if true.
*/
int		ft_isprint(int c);

/* @brief Checks if the int 'c' is a whitespace
 *
 * @param c the character
 * @return int 0 if false and 1 if true.
*/
int		ft_isspace(int c);

#endif
