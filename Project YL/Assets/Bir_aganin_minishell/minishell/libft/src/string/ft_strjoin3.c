/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   ft_strjoin3.c                                      :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2024/10/16 18:36:28 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/14 14:37:48 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/libft.h"

static char	*join3(const char *s1, const char *s2, const char *s3, char *res);

char	*ft_strjoin3(char const *s1, char const *s2, char const *s3)
{
	char		*res;

	res = (char *)malloc(sizeof(char) * ft_strlen(s1) + ft_strlen(s2)
			+ ft_strlen(s3) + 1);
	if (!res)
		return (NULL);
	return (join3(s1, s2, s3, res));
}

static char	*join3(const char *s1, const char *s2, const char *s3, char *res)
{
	size_t		i;
	size_t		j;

	i = 0;
	while (s1[i] != '\0')
	{
		res[i] = s1[i];
		i++;
	}
	j = 0;
	while (s2[j] != '\0')
	{
		res[i] = s2[j];
		i++;
		j++;
	}
	j = 0;
	while (s3[j] != '\0')
	{
		res[i] = s3[j];
		i++;
		j++;
	}
	res[i] = '\0';
	return (res);
}
